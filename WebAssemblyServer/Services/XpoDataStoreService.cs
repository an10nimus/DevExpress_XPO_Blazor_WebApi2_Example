using System;
using System.IO;
using System.Xml.Serialization;

using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.Xpo.Exceptions;
using DevExpress.Xpo.Helpers;

using Microsoft.Extensions.Configuration;

namespace WebAssemblyServer.Services {
    public class XpoDataStoreService {
        public static XpoDataStoreService Create(IServiceProvider serviceProvider) {
            IConfiguration config = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
            string connectionString = config["XpoDataStore:ConnectionString"];
            AutoCreateOption autoCreateOption = (AutoCreateOption)Enum.Parse(typeof(AutoCreateOption), config["XpoDataStore:AutoCreateOption"]);
            bool useDataStorePool = bool.Parse(config["XpoDataStore:UseDataStorePool"]);
            return new XpoDataStoreService(connectionString, autoCreateOption, useDataStorePool);
        }
        public IDataStore DataStore;
        XpoDataStoreService(string connectionString, AutoCreateOption autoCreateOption, bool useDataStorePool) {
            if(useDataStorePool)
                connectionString = XpoDefault.GetConnectionPoolString(connectionString);
            DataStore = XpoDefault.GetConnectionProvider(connectionString, autoCreateOption);
        }
        public string Handle(string method, string args) {
            if(string.IsNullOrEmpty(method)) {
                return string.Empty;
            }
            switch(method.ToLowerInvariant()) {
                case "autocreateoption":
                    return Serialize(Execute(() => DataStore.AutoCreateOption));
                case "do":
                case "doasync": {
                        string[] argStrings = args.Split('|');
                        return Serialize(Execute(() => ((ICommandChannel)DataStore).Do(Deserialize<string>(argStrings[0]), Deserialize<object>(argStrings[1]))));
                    }
                case "modifydata":
                case "modifydataasync":
                    return Serialize(Execute(() => DataStore.ModifyData(Deserialize<ModificationStatement[]>(args))));
                case "selectdata":
                case "selectdataasync":
                    return Serialize(Execute(() => DataStore.SelectData(Deserialize<SelectStatement[]>(args))));
                case "updateschema": {
                        string[] argStrings = args.Split('|');
                        return Serialize(Execute(() => DataStore.UpdateSchema(Deserialize<bool>(argStrings[0]), Deserialize<DBTable[]>(argStrings[1]))));
                    }
            }
            throw new NotSupportedException($"Method not suppoted: '{method}'.");
        }
        static readonly Type[] knownTypes = {
            typeof(DeleteStatement),
            typeof(InsertStatement),
            typeof(UpdateStatement),
            typeof(AggregateOperand),
            typeof(BetweenOperator),
            typeof(BinaryOperator),
            typeof(ContainsOperator),
            typeof(FunctionOperator),
            typeof(GroupOperator),
            typeof(InOperator),
            typeof(NotOperator),
            typeof(NullOperator),
            typeof(OperandProperty),
            typeof(OperandValue),
            typeof(ParameterValue),
            typeof(QueryOperand),
            typeof(UnaryOperator),
            typeof(JoinOperand),
            typeof(OperandParameter),
            typeof(QuerySubQueryContainer),
            typeof(CommandChannelHelper.SprocQuery),
            typeof(CommandChannelHelper.SqlQuery),
            typeof(DBTable)
        };
        static T Deserialize<T>(string result) {
            if(string.IsNullOrEmpty(result)) {
                return default(T);
            }
            string incoming = result;
            byte[] resultBytes = Convert.FromBase64String(incoming);
            using(var stream = new MemoryStream(resultBytes)) {
                var serializer = new XmlSerializer(typeof(T), knownTypes);
                return (T)serializer.Deserialize(stream);
            }
        }
        static readonly char[] padding = { '=' };
        static string Serialize<T>(T value) {
            using(var stream = new MemoryStream()) {
                var serializer = new XmlSerializer(typeof(T), knownTypes);
                serializer.Serialize(stream, value);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        static OperationResult<T> Execute<T>(OperationResultPredicate<T> predicate) {
            try {
                return new OperationResult<T>(predicate());
            } catch(NotSupportedException ex) {
                return new OperationResult<T>(ServiceException.NotSupported, ex.Message);
            } catch(SchemaCorrectionNeededException ex) {
                return new OperationResult<T>(ServiceException.Schema, ex.Message);
            } catch(LockingException) {
                return new OperationResult<T>(ServiceException.Locking, string.Empty);
            } catch(ObjectLayerSecurityException ex) {
                return new OperationResult<T>(ServiceException.ObjectLayerSecurity, OperationResult.SerializeSecurityException(ex));
            } catch(Exception ex) {
                return new OperationResult<T>(ServiceException.Unknown, ex.Message);
            }
        }

    }
}
