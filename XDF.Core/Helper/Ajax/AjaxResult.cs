using System;

namespace XDF.Core.Helper.Ajax
{
    public static class AjaxResult
    {
        #region Success
        public static AjaxResultModel<T> Success<T>(string msg)
        {
            return new AjaxResultModel<T> {Msg = msg, Code = 1, IsSuccess = true};
        }
        public static AjaxResultModel<TData> Success<TData>(TData data)
        {
            return new AjaxResultModel<TData> {Data = data, IsSuccess = true, Code = 1};
        }
        public static AjaxResultModel<TData> Success<TData>(string msg, TData data)
        {
            return new AjaxResultModel<TData> {Msg = msg, Data = data, IsSuccess = true, Code = 1};
        }
        public static AjaxResultModel<TData> Success<TData>(string msg, int code, TData data)
        {
            return new AjaxResultModel<TData> {Msg = msg, Data = data, Code = code, IsSuccess = true};
        }
        public static AjaxResultModel<TData> Success<TData>(int code, TData data) where TData : new()
        {
            return new AjaxResultModel<TData> {Code = code, Data = data, Msg = "", IsSuccess = true};
        }
      
        public static AjaxResultModel<T> Success<T>(int code, string msg)
        {
            return new AjaxResultModel<T> {Code = code, Msg = msg, IsSuccess = true};
        }
        public static AjaxResultModel<TData> Success<TData>(int code, string msg, TData data)
        {
            return new AjaxResultModel<TData> {Code = code, Data = data, Msg = msg, IsSuccess = true};
        }
        #endregion
        #region Error
        public static AjaxResultModel<T> Error<T>(string msg)
        {
            return new AjaxResultModel<T> {Msg = msg, Code = -1, IsSuccess = false};
        }
        public static AjaxResultModel<T> Error<T>(T data)
        {
            return new AjaxResultModel<T> { Data = data, Code = -1, IsSuccess = false };
        }
        public static AjaxResultModel<TData> Error<TData>(string msg, TData data)
        {
            return new AjaxResultModel<TData> {Msg = msg, Data = data, Code = -1, IsSuccess = false};
        }
        public static AjaxResultModel<TData> Error<TData>(int code, string msg,TData data)
        {
            return new AjaxResultModel<TData> {Msg = msg, Data = data, Code = code, IsSuccess = false};
        }
        public static AjaxResultModel<T> Error<T>(int code, string msg)
        {
            return new AjaxResultModel<T> {Code = code, Msg = msg, IsSuccess = false};
        }
     
        #endregion
    }
}