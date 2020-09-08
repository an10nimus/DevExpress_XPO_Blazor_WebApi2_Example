using System.Collections.Generic;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using DevExpress.Xpo.Metadata;
using System.ComponentModel;
using System;

namespace WebAssemblySource.Shared {
    public class InputSelectXpoObject<TObject> : InputBase<TObject> {
        const string EmptyValueMagicKeyword = "#EMPTY#";
        [Parameter] public IList<TObject> DataSource { get; set; }
        [Parameter] public string DisplayMember { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            builder.OpenElement(0, "select");
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            builder.AddAttribute(3, "class", CssClass);
            builder.AddAttribute(4, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString, null));
            builder.OpenElement(6, "option");
            builder.AddAttribute(7, "value", EmptyValueMagicKeyword);
            builder.AddContent(8, "None");
            builder.CloseElement();
            int seqNum = 9;
            foreach(TObject obj in DataSource) {
                builder.OpenElement(seqNum++, "option");
                builder.AddAttribute(seqNum++, "value", GetClassInfo(obj).KeyProperty.GetValue(obj)?.ToString());
                builder.AddContent(seqNum++, GetDisplayText(obj));
                builder.CloseElement();
            }
            builder.CloseElement();
        }
        protected override bool TryParseValueFromString(string value, out TObject result, out string validationErrorMessage) {
            if(string.IsNullOrEmpty(value) || value == EmptyValueMagicKeyword) {
                result = default;
                validationErrorMessage = string.Empty;
                return true;
            } else {
                foreach(TObject obj in DataSource) {
                    string keyValue = GetClassInfo(obj).KeyProperty.GetValue(obj)?.ToString();
                    if(value == keyValue) {
                        result = obj;
                        validationErrorMessage = string.Empty;
                        return true;
                    }
                }
                result = default;
                validationErrorMessage = $"Cannot find object by key in the DataSource. Key: '{value}'.";
                return false;
            }
        }
        protected override string FormatValueAsString(TObject value) {
            if(value == null)
                return EmptyValueMagicKeyword;
            return GetClassInfo(value).KeyProperty.GetValue(value)?.ToString();
        }
        string GetDisplayText(TObject obj) {
            if(string.IsNullOrWhiteSpace(DisplayMember)) {
                DefaultPropertyAttribute attr = (DefaultPropertyAttribute)GetClassInfo(obj).FindAttributeInfo(typeof(DefaultPropertyAttribute));
                if(attr != null) {
                    XPMemberInfo defaultMember = GetClassInfo(obj).FindMember(attr.Name);
                    if(defaultMember != null)
                        return defaultMember.GetValue(obj)?.ToString();
                }
                return obj.ToString();
            } else {
                XPMemberInfo member = GetClassInfo(obj).FindMember(DisplayMember);
                if(member == null)
                    return obj.ToString();
                else
                    return member.GetValue(obj)?.ToString();
            }
        }
        XPClassInfo GetClassInfo(TObject obj) {
            IXPSimpleObject simpleObj = obj as IXPSimpleObject;
            if(obj == null)
                throw new ArgumentException("DataSource does not contain XPO objects.");
            return simpleObj.ClassInfo;
        }
    }
}
