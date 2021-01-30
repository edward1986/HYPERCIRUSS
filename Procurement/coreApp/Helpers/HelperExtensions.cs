using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace System.Web.Helpers
{
    public static class HelperExtensions
    {
        public static MvcHtmlString Checkbox_WithLabel(this HtmlHelper helper, string name, string value, string label, bool isChecked = false, bool disabled = false, bool customValues = true, bool noHiddenField = false, bool inline = false)
        {
            string hiddenField = "<input type=\"hidden\" name=\"{1}\" value=\"{2}\" />";
            string template = "<div class=\"checkbox {0}\"" + (inline ? " style=\"display:inline-block\"" : "") + "><label>" + (noHiddenField ? "" : hiddenField) + "<input type=\"checkbox\" name=\"{1}\" value=\"{2}\" {0} {4} {5}>{3}</label></div>";
            string _disabled = disabled ? "disabled" : "";
            string _checked = isChecked ? "checked" : "";
            string _customValues = customValues ? "custom-values" : "";

            string html = string.Format(template, _disabled, name, value, label, _checked, _customValues);

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString CheckboxFor_WithLabel<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string namePrefix = "", bool disabled = false)
        {
            ModelMetadata m = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData);
            var propName = m.PropertyName;
            var dispName = m.DisplayName;
            bool value = (bool)m.Model;

            string template = "<div class=\"checkbox {0}\"><label><input type=\"hidden\" name=\"{1}\" value=\"{2}\" /><input type=\"checkbox\" name=\"{1}\" value=\"{2}\" {0} {4}>{3}</label></div>";
            string _disabled = disabled ? "disabled" : "";
            string _checked = value ? "checked" : "";
            string _name = (string.IsNullOrEmpty(namePrefix) ? "" : namePrefix + ".") + propName;
            string _value = value.ToString().ToLower();

            string html = string.Format(template, _disabled, _name, _value, dispName, _checked);

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString DisplayBooleanFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata m = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData);
            var propName = m.PropertyName;
            var dispName = m.DisplayName;
            bool value = Convert.ToBoolean(m.Model);

            string html = "<i class=\"feather icon-x\"></i>";

            if (value)
            {

                html = "<i class=\"feather icon-check\"></i>";
            }
            
            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString DisplayMultiLineFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata m = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData);
            var propName = m.PropertyName;
            var dispName = m.DisplayName;
            string value = (string)m.Model;

            string html = "";
            if (!string.IsNullOrEmpty(value))
            {
                html = "<div>" + value.Replace("\n", "<br />") + "</div>";
            }


            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString DisplayMultiLine(this HtmlHelper helper, string expression)
        {
            string html = "";
            if (!string.IsNullOrEmpty(expression))
            {
                html = "<div>" + expression.Replace("\n", "<br />") + "</div>";
            }


            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString ToDateTimeString<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata m = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData);
            var propName = m.PropertyName;
            var dispName = m.DisplayName;
            DateTime value = (DateTime)m.Model;

            string html = "";
            if (value != null)
            {
                html = value.ToShortDateString() + "<br />" + value.ToShortTimeString();
            }


            return MvcHtmlString.Create(html);
        }
    }
}