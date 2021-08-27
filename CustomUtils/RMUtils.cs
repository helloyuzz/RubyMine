using RubyMine.Models;
using RubyMine.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubyMine {
    public class RMUtils {
        public static string ParseMailNotification(string value) {
            string result = "未知";
            switch (value) {
                case "all":
                    result = "所有";
                    break;
                case "only_my_events":
                    result = "仅收取我关注的项目";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        internal static List<RMUser> ParseToRMUser(IList<User> db_Project, List<CustomField> db_CustomFields, List<CustomValue> db_CustomValues) {
            List<RMUser> RMProjects = new List<RMUser>();
            foreach (User db_User in db_Project) {
                RMUser to_User = new RMUser();
                foreach (PropertyInfo info in typeof(User).GetProperties()) {
                    info.SetValue(to_User, info.GetValue(db_User));
                }
                ParseToRMUser(db_User, db_CustomFields, db_CustomValues,to_User);
                
                RMProjects.Add(to_User);
            }
            return RMProjects;
        }
        internal static List<RMIssue> ParseToRMIssues(IList<Issue> db_Issues, List<CustomField> db_CustomFields, List<CustomValue> db_CustomValues) {
            List<RMIssue> RMIssues = new List<RMIssue>();
            foreach (Issue db_Issue in db_Issues) {
                RMIssue to_Issue = new RMIssue();
                foreach (PropertyInfo info in typeof(Issue).GetProperties()) {
                    info.SetValue(to_Issue, info.GetValue(db_Issue));
                }
                ParseToRMIssus(db_Issue, db_CustomFields, db_CustomValues, to_Issue);

                RMIssues.Add(to_Issue);
            }
            return RMIssues;
        }

        internal static object SHA1(string orignalPassword) {
            var strRes = Encoding.Default.GetBytes(orignalPassword);
            HashAlgorithm iSha = new SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var encrypt_Password = new StringBuilder();
            foreach (byte iByte in strRes) {
                encrypt_Password.AppendFormat("{0:x2}", iByte);
            }
            return encrypt_Password.ToString();
        }

        internal static int ParseInt(object value) {
            if (value == null || value == DBNull.Value) {
                return 0;
            }

            int intValue = 0;
            int.TryParse(value.ToString(), out intValue);
            return intValue;
        }
        static RMIssue ParseToRMIssus(Issue db_Issue, List<CustomField> db_CustomFields, List<CustomValue> db_CustomValues, RMIssue to_Issue = null) {
            if (to_Issue == null) {
                to_Issue = new RMIssue();
            }
            foreach (CustomField field in db_CustomFields) {
                // 复制CustomField1
                CustomField1 customField1 = new CustomField1();
                foreach (PropertyInfo info in typeof(CustomField).GetProperties()) {
                    info.SetValue(customField1, info.GetValue(field));
                }

                // 复制CustomValue
                List<CustomValue> values = db_CustomValues.Where(x => x.CustomFieldId == field.Id && x.CustomizedId == db_Issue.Id).ToList();
                if (values.Count > 0) {
                    customField1.CustomValue = values[0];
                }
                if (to_Issue.CustomFields == null) {
                    to_Issue.CustomFields = new List<CustomField1>();
                }
                to_Issue.CustomFields.Add(customField1);
            }
            return to_Issue;
        }

        internal static RMUser ParseToRMUser(User db_User, List<CustomField> db_CustomFields, List<CustomValue> db_CustomValues,RMUser to_User=null) {
            if (to_User == null) {
                to_User = new RMUser();
            }
            foreach (CustomField field in db_CustomFields) {
                // 复制CustomField1
                CustomField1 customField1 = new CustomField1();
                foreach (PropertyInfo info in typeof(CustomField).GetProperties()) {
                    info.SetValue(customField1, info.GetValue(field));
                }

                // 复制CustomValue
                List<CustomValue> values = db_CustomValues.Where(x => x.CustomFieldId == field.Id && x.CustomizedId == db_User.Id).ToList();
                if (values.Count > 0) {
                    customField1.CustomValue = values[0];
                }
                if (to_User.CustomFields == null) {
                    to_User.CustomFields = new List<CustomField1>();
                }
                to_User.CustomFields.Add(customField1);
            }
            return to_User;
        }
        internal static void CloneObject<T, V>(T fromObj, V toObj) {
            foreach (PropertyInfo info in fromObj.GetType().GetProperties()) {
                info.SetValue(toObj, info.GetValue(fromObj));
            }
        }
        public static string ParseUserStatus(int status) {
            return status == 1 ? "激活的" : "已锁定";
        }
        public static string ParseDate(DateTime? dateTime,bool showMintues=false) {
            if (showMintues == true) {
                return dateTime.Value.ToString("yy-MM-dd HH:mm");
            } else {
                return dateTime.Value.ToString("yyyy-MM-dd");
            }
        }
        public static string PraseDateTime(DateTime? dateTime) {
            return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ParseCustomField(CustomField1 customField1) {
            string result = "";
            switch (customField1.FieldFormat) {
                case "bool":
                    result = ParseBool(customField1.CustomValue.Value);
                    break;
                default:
                    result = customField1.CustomValue.Value;
                    break;
            }
            return result;
        }

        public static string ParseBool(string value) {
            string result = "否";
            switch (value.ToLower()) {
                case "":
                    result = "N/A";
                    break;
                case "1":
                case "true":
                    result = "是";
                    break;
            }
            return result;
        }
        public static string ParseUserName(User user=null,RMUser rMUser=null) {
            string userName = "";

            if (user != null) {
                userName = user.Lastname + user.Firstname;
            }
            if (rMUser != null) {
                userName = rMUser.Lastname + rMUser.Firstname;
            }

            return userName;
        }
        public static string Html_IsRequired(bool isRequired) {
            return isRequired ? "&nbsp;<span class=\"text-danger fw-bold\">*</span>" : "";
        }
        public static string Html_Checked(string boolValue) {
            return boolValue.Equals("1") ? "checked" : "";
        }
    }
}
