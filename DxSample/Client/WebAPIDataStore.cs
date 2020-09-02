﻿using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.Xpo.Exceptions;
using DevExpress.Xpo.Helpers;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text.Json;

namespace DxSample.Client {
    internal class WebAPIDataStore : IDataStore, IDataStoreAsync {
        static HttpClient client = new HttpClient();
        private string url;
        public const string XpoProviderTypeString = "WebAPIDataStore";
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
        public AutoCreateOption AutoCreateOption => AutoCreateOption.DatabaseAndSchema;

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

        static string Serialize<T>(T value) {
            using(var stream = new MemoryStream()) {
                var serializer = new XmlSerializer(typeof(T), knownTypes);
                serializer.Serialize(stream, value);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        static T Deserialize<T>(string result) {
            if(string.IsNullOrEmpty(result)) {
                return default(T);
            }
            byte[] resultBytes = Convert.FromBase64String(result);
            using(var stream = new MemoryStream(resultBytes)) {
                var serializer = new XmlSerializer(typeof(T), knownTypes);
                return (T)serializer.Deserialize(stream);
            }
        }

        public WebAPIDataStore(string url) {
            this.url = url;
            client.BaseAddress = new Uri(this.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ModificationResult> ModifyDataAsync(CancellationToken cancellationToken, params ModificationStatement[] dmlStatements) {
            cancellationToken.ThrowIfCancellationRequested();
            var serialized_modstatement = Serialize<ModificationStatement[]>(dmlStatements);
            XPOContentHolder obj2 = new XPOContentHolder() {
                content = serialized_modstatement
            };
            var content = new StringContent(JsonSerializer.Serialize(obj2), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"api/xpo/modifydataasync", content);
            response.EnsureSuccessStatusCode();
            var some_var = await response.Content.ReadAsStringAsync();
            XPOContentHolder obj = JsonSerializer.Deserialize<XPOContentHolder>(some_var);
            ModificationResult response_deserialized = Deserialize<OperationResult<ModificationResult>>(obj.content).Result;
            return response_deserialized;
        }

        public class XPOContentHolder {
            public string content { get; set; }
        }

        public async Task<UpdateSchemaResult> UpdateSchemaAsync(CancellationToken cancellationToken, bool doNotCreateIfFirstTableNotExist, params DBTable[] tables) {
            cancellationToken.ThrowIfCancellationRequested();
            var serialized_data = Serialize<DBTable[]>(tables);
            var serialized_bool = Serialize<bool>(doNotCreateIfFirstTableNotExist);
            XPOContentHolder obj2 = new XPOContentHolder() { 
                content = serialized_bool + '|' + serialized_data
            };
            var content = new StringContent(JsonSerializer.Serialize(obj2), Encoding.UTF8, "application/json");
            try { 
                HttpResponseMessage response = await client.PostAsync($"api/xpo/updateschema", content);
                response.EnsureSuccessStatusCode();
                var some_var = await response.Content.ReadAsStringAsync();
                XPOContentHolder obj = JsonSerializer.Deserialize<XPOContentHolder>(some_var);
                UpdateSchemaResult response_deserialized = Deserialize<OperationResult<UpdateSchemaResult>>(obj.content).Result;
                return response_deserialized;
            } catch(Exception ex) {
                Trace.WriteLine(ex.Message);
                throw;
            }            
        }

        public async Task<SelectedData> SelectDataAsync(CancellationToken cancellationToken, params SelectStatement[] selects) {
            cancellationToken.ThrowIfCancellationRequested();
            var serialized_selects = Serialize<SelectStatement[]>(selects);
            XPOContentHolder obj2 = new XPOContentHolder() {
                content = serialized_selects
            };
            var content = new StringContent(JsonSerializer.Serialize(obj2), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"api/xpo/selectdataasync", content);
            response.EnsureSuccessStatusCode();
            var some_var = await response.Content.ReadAsStringAsync();
            XPOContentHolder obj = JsonSerializer.Deserialize<XPOContentHolder>(some_var);
            SelectedData response_deserialized = Deserialize<OperationResult<SelectedData>>(obj.content).Result;
            return response_deserialized;
        }

        public ModificationResult ModifyData(params ModificationStatement[] dmlStatements) {
            throw new NotImplementedException();
        }

        public UpdateSchemaResult UpdateSchema(bool doNotCreateIfFirstTableNotExist, params DBTable[] tables) {
                throw new NotImplementedException();
            }

        public SelectedData SelectData(params SelectStatement[] selects) {
            throw new NotImplementedException();
        }
    }
}