using SevenTiny.Bantina.Security;
using System;
using Xunit;

namespace Test.Seventiny.Cloud.Account.Core
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void GeneratePassword()
        {
            //Ϊ���ͱ����ƽ�Ŀ��ܣ�����ǿ��ǰ�����
            string password = "123456";
            var pwd = MD5Helper.GetMd5Hash(string.Concat("seventiny.cloud.account.salt.", password, ".25913AEE-8F27-49DB-89AA-AD730CAB58F1"));
        }
    }
}
