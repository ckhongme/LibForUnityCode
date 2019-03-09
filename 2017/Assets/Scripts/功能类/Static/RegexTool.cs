using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace K
{
    /// <summary>
    /// 正则表达式
    /// </summary>
    public class RegexTool
    {
        #region 匹配方法

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">是否区分大小写</param>  
        /// <param name="ifIgnoreEmpty">是否验证空白字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase = false, bool ifIgnoreEmpty = false)
        {
            if (!ifIgnoreEmpty && string.IsNullOrEmpty(inputStr))
                return false; //空白字符串不匹配  

            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr);
        }

        #endregion

        #region Passord 密码

        /// <summary>  
        /// 验证只包含英文字母  
        /// </summary>  
        public static bool IsOnlyEN(string input)
        {
            string pattern = @"^[A-Za-z]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含汉字  
        /// </summary>  
        public static bool IsOnlyCN(string input)
        {
            string pattern = @"^[\u4e00-\u9fa5]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含数字和英文字母  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsNumAndEn(string input)
        {
            string pattern = @"^[0-9A-Za-z]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>
        /// 验证强类型密码  (必须包含大小写字母和数字的组合，不能使用特殊字符，长度在6-16之间)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStrongPwd(string input)
        {
            string pattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,16}$";
            return IsMatch(input, pattern);
        }

        #endregion

        /// <summary>  
        /// 验证身份证号（不区分一二代身份证号）  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard(string input)
        {
            if (input.Length == 18)
                return IsIDCard18(input);
            else if (input.Length == 15)
                return IsIDCard15(input);
            else
                return false;
        }

        /// <summary>  
        /// 验证一代身份证号（15位数）  
        /// </summary>  
        private static bool IsIDCard15(string input)
        {
            string pattern = @"/^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证二代身份证号（18位数，GB11643-1999标准）  
        /// </summary> 
        private static bool IsIDCard18(string input)
        {
            string pattern = @"/^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4}$/";
            return IsMatch(input, pattern);
        }


        /// <summary>  
        /// 验证电子邮箱  
        /// </summary>  
        /// [@字符前可以包含字母、数字、下划线和点号；@字符后可以包含字母、数字、下划线和点号；@字符后至少包含一个点号且点号不能是最后一个字符；最后一个点号后只能是字母或数字]  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsEmail(string input)
        {
            ////邮箱名以数字或字母开头；邮箱名可由字母、数字、点号、减号、下划线组成；邮箱名（@前的字符）长度为3～18个字符；邮箱名不能以点号、减号或下划线结尾；不能出现连续两个或两个以上的点号、减号。  
            //string pattern = @"^[a-zA-Z0-9]((?<!(\.\.|--))[a-zA-Z0-9\._-]){1,16}[a-zA-Z0-9]@([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?|((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$";  
            string pattern = @"^([\w-\.]+)@([\w-\.]+)(\.[a-zA-Z0-9]+)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证手机号码  
        /// </summary>  
        /// /// [可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，11位手机号前的0可以省略；11位手机号第二位数可以是3、4、5、8中的任意一个]  
        public static bool IsPhoneNum(string input)
        {
            string pattern = @"^((\+)?86|((\+)?86)?)0?1[3458]\d{9}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证日期  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsDateTime(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证邮政编码  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsZipCode(string input)
        {
            string pattern = @"^\d{6}$";
            return IsMatch(input, pattern);
        }


        #region 验证数据格式

        /// <summary>  
        /// 验证数字(double类型)  
        /// [可以包含负号和小数点]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsNumber(string input)
        {
            string pattern = @"^-?\d+$|^(-?\d+)(\.\d+)?$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsInteger(string input)
        {
            string pattern = @"^-?\d+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证非负整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerNotNagtive(string input)
        {
            string pattern = @"^\d+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证正整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerPositive(string input)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证小数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsDecimal(string input)
        {
            string pattern = @"^([-+]?[1-9]\d*\.\d+|-?0\.\d*[1-9]\d*)$";
            return IsMatch(input, pattern);
        }

        #endregion
    }
}


/* Test
 
        Debug.Log("email  " + RegexTool.IsEmail("ckhonglogin@163.com"));
        Debug.Log("email  " + RegexTool.IsEmail("ckhonglogin@163com"));

        Debug.Log("phone  " + RegexTool.IsPhoneNum("13642220137"));
        Debug.Log("phone  " + RegexTool.IsPhoneNum("1364222017"));

        Debug.Log("onlyCN  " + RegexTool.IsOnlyCN("小凯"));
        Debug.Log("onlyCN  " + RegexTool.IsOnlyCN("小凯11"));

        Debug.Log("onlyEN  " + RegexTool.IsOnlyEN("eeeee"));
        Debug.Log("onlyEN  " + RegexTool.IsOnlyEN("eddd1"));

        Debug.Log("StrongPwd   " + RegexTool.IsStrongPwd("KSDFKJsdfs123423"));
        Debug.Log("StrongPwd   " + RegexTool.IsStrongPwd("KSDFKJ111"));
        Debug.Log("StrongPwd   " + RegexTool.IsStrongPwd("aaaaa111"));
 */
