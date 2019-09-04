using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace MyHttpClient
{
    class Program
    {
        //模拟客户端向http服务器发起http请求
        static void Main(string[] args)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            string contentType = "application/json";
            string response = HttpHelp.CreateRequestHttpForPOST("http://127.0.0.1:8888", AuditRequest(), Encoding.UTF8, contentType, null);
            //JObject jo = JObject.Parse(response);
        }

        /// <summary>
        /// 2006002 - 核保请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string AuditRequest()
        {
            //请求的类型:  2006002 - 核保请求
            JObject jAudit = new JObject();//最外层JSON
            JObject jHeader = new JObject
            {
                { "requestTime", DateTime.Now.ToString("yyyy-MM-dd hh:mi:ss") },
                { "transType", "2006002" },
                { "requestId", DateTime.Now.ToString("yyyyMMddhhmiss") },
                { "preferLangId","211" }
            };


            JObject jRequest = new JObject
            {
                { "addresses", new JObject {
                     {"address1", "省份"},
                     {"address2", "市区"},
                     {"address3", "城区"},
                     {"address4", "详细地址"},
                     {"addressId", "id,判断该地址是什么人的地址(唯一标识)"},
                     {"postCode", "邮政编码" }}
                },
                { "customers", new JObject {
                     {"partyId" , "人的唯一标识"},
                     {"foreignIndi" , "foreignIndi"},
                     {"partyContact" , new JObject {
                            { "email", "xinchao.yuan@techceate.net"},
                            { "mobile", "13800138000"},
                            { "officeAreaCode", "010-123456789"},
                            { "officetel", "010-987654321"},
                            { "homeAreaCode", "家庭电话区号010"},
                            { "homeTel", "家庭电话87654321"}}
                     },
                     {"person" , new JObject {
                            {"PersonInputFounder", "类名"},
                            {"birthday", "生日"},
                            {"certiCode", "证件号"},
                            {"certiType", "证件类型"},
                            {"certiEndDate", "证件有效截至日"},
                            {"firstName", "姓名"},
                            {"gender", "性别"},
                            {"height", "身高"},
                            {"weight", "体重"},
                            {"jobCode", "职业"},
                            {"jobContent", "职业名称"},
                            {"smoking", "是否抽烟"},
                            {"marriageId", "婚姻类型"},
                            {"countryCode", "国籍"},
                            {"countryName", "国籍名称"},
                            {"socialInsuranceFlag", "社保"},
                            {"monthIncome", "月收入"}}
                     },
                     {"organization" , new JObject {
                            { "OrganizationInputFounder", "OrganizationInputFounder"},
                            { "taxIdentity", "1"},
                            { "editTime", DateTime.Now.ToString("yyyy-MM-dd")},
                            { "birthday", "1990-01-28"}}
                     }}
                },
                { "policy", new JObject {
                            {"PolicyInputFounder", ""},
                            {"waiverFlag","豁免险"},
                            {"prePrintNo","打印凭证"},
                            {"applyDate","保单请求时间"},
                            {"proposalNumber","投保单号"},
                            {"inceptionDate","生效日"},
                            {"expiryDate","到期日"},
                            {"benefitShareType","受益人类型"},
                            {"currencys","币种"},
                            {"autoAplIndi","保费过期选择"},
                            {"renewalOneYearForTrm","	一年期是否续保标志"},
                            {"renewInvoicePostWay","续期保费发票处理"},
                            {"serviceAgentCode","代理人编号"},
                            {"serviceAgentName","代理人姓名"},
                            {"policyPrem","支付金额"},
                            {"policyPrintType","打印类型"},
                            {"coverages ",new JObject{
                                {"CoverageInputFounder","CoverageInputFounder"},
                                {"chargePeriod","缴费类型"},
                                {"chargeYear","缴费年限"},
                                {"coveragePeriod","保期类型"},
                                {"coverageYear","保障年限"},
                                {"paymentFreq","缴费类型"},
                                {"premium","保费"},
                                {"sumAssured","保额"},
                                {"productCode","产品编号"},
                                {"unit","份数"},
                                {"coverageId","产品顺序"},
                                {"serviceAgentCode","代理人编号"},
                                {"masterCoverageId","对应主险的coverageId"},
                                {"payPlans", new JObject{{ "payOption","领取方式类别"}, {"planType","领取方式"}}},
                                {"insureds", new JObject{
                                    {"addressId","addressId"},
                                    {"partyId","partyId"},
                                    {"listId","被保人顺序标识"},
                                    {"relationToPolicyHolder","被保人与投保人关系"},
                                    {"relationToMainInsured","被保人与主被保人关系"}
                                } },
                                {"insuredId","险种被保人id"},
                                {"orderId","表示险种下被保人的顺序"},
                                {"jobClass","被保人的职业类型"},
                            } },
                            {"policyHolder",new JObject{
                                {"addressId","addressId" },
                                {"partyId","partyId"},
                                {"relationToInsured","relationToInsured"}
                            }},
                            {"insureds",new JObject{
                                 {"addressId","addressId"},
                                 {"partyId","partyId"},
                                 {"listId","被保人顺序标识"},
                                 {"relationToPolicyHolder","被保人与投保人关系"},
                                 {"relationToMainInsured","被保人与主被保人关系"}
                            } },
                            {"beneficiaries",new JObject{
                                {"partyId","partyId"},
                                {"beneType","受益人类型"},
                                {"legalBeneIndi","是否是法定受益人"},
                                {"relationToInsured","受益人与被保人关系"},
                                {"shareOrder","受益顺序"},
                                {"shareRate","受益比例"}
                            } },
                            {"declarations",new JObject{
                                {"partyId","标识某个人的健康告知"},
                                {"declarationDate","健康告知的日期"},
                                {"declarationDetails","详细健康告知项"},
                                {"declId","健康告知项标识"},
                                {"fill1","答案"}
                            } }}
                }
            };

            jAudit.Add("transHeader", jHeader);
            jAudit.Add("transRequest", jRequest);
            return jAudit.ToString(Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        ///  2006003 - 承保请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static Dictionary<string, string> EffectiveRequest(Dictionary<string, string> param)
        {
            //请求的类型:  2006003 - 承保请求
            param.Add("name", "yxc");
            param.Add("age", "29");
            param.Add("gender", "m");
            return param;
        }

        /// <summary>
        /// 2006012 - 获取保单号请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetApplicationNumRequest(Dictionary<string, string> param)
        {
            //请求的类型: 2006012 - 获取保单号请求
            param.Add("name", "yxc");
            param.Add("age", "29");
            param.Add("gender", "m");
            return param;
        }

        /// <summary>
        /// 1006001 - 保费试算
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static Dictionary<string, string> PremiumCalcRequest(Dictionary<string, string> param)
        {
            //请求的类型: 1006001 - 保费试算
            param.Add("name", "yxc");
            param.Add("age", "29");
            param.Add("gender", "m");
            return param;
        }

        /// <summary>
        /// 2006004 - 保单状态同步
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static Dictionary<string, string> PolicySyncRequest(Dictionary<string, string> param)
        {
            //请求的类型:2006004 - 保单状态同步
            param.Add("name", "yxc");
            param.Add("age", "29");
            param.Add("gender", "m");
            return param;
        }
    }
}
