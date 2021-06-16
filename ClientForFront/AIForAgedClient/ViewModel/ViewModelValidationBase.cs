using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AIForAgedClient.ViewModel
{
    /// <summary>
    /// 带有数据校验功能的通知基类
    /// </summary>
    public class ViewModelValidationBase : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// 需要校验的属性集合
        /// </summary>
        private List<string> validationKeys = new List<string>();

        /// <summary>
        /// 错误信息字典
        /// </summary>
        private Dictionary<string, string> errorDic = new Dictionary<string, string>();

        /// <summary>
        /// 是否开始校验，以此控制IDataErrorInfo接口校验的时候界面刚初始化即开始校验
        /// </summary>
        public bool IsBeginValidation { get; set; }

        public string this[string columnName]
        {
            get
            {
                if (!IsBeginValidation)
                {
                    return string.Empty;
                }
                var vc = new ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), vc, res);
                if (res.Count > 0)
                {
                    string errorMessage = string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                    AddErrorDic(columnName, errorMessage);
                    return errorMessage;
                }
                RemoveErrorDic(columnName);
                return string.Empty;
            }
        }

        public string Error => string.Join(Environment.NewLine, errorDic.Values.ToArray());

        /// <summary>
        /// 添加进行校验的属性集合
        /// </summary>
        /// <param name="keys"></param>
        protected virtual void ValidationKey(string[] keys)
        {
            if (keys == null)
            {
                return;
            }
            foreach (var item in keys)
            {
                validationKeys.Add(item);
            }
        }

        /// <summary>
        /// 强制进行以此校验
        /// </summary>
        protected void Validation()
        {
            if (validationKeys != null)
            {
                foreach (var item in validationKeys)
                {
                    RaisePropertyChanged(item);
                }
            }
        }

        private void AddErrorDic(string key, string value)
        {
            if (!errorDic.Keys.Contains(key))
            {
                errorDic.Add(key, value);
            }
        }

        private void RemoveErrorDic(string key)
        {
            errorDic.Remove(key);
        }
    }
}