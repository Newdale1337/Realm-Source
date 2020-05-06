using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

namespace Externals.Cryptography
{
    public class RSA
    {
        public static RSA Instance = new RSA();

        private RsaEngine RSAEngine { get; set; }
        private AsymmetricKeyParameter Key { get; set; }

        public RSA()
        {
            var privPem = @"
-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgHOtmFFAtby5/t3a7F7PGwr6ZRG7SWx439P9KffZfBIp5Sg2LJY5
O9w5SQ14O7h2es/ZbwJB7xS2QbEbGWTeDq+6xl7Y72yzXHBvLqYit3uZyflFSisu
NgfN4Z/nAdli1u3yW7+w6dSL14KcLeDnrs3iJjVyqekEpryGyPwB9QsRAgMBAAEC
gYA2PpVKpNmUInQNfPeSjfPUdg6m/fg0UYpEUlc3zliL+/FlpeHKoBQd9Q74rgTz
Pzvf88pGeLywLbcoYdjKoAaqZGWSpfcpnbJJpG98LK56ZbiH9tOp4pi5l8f0wtn+
1RVfq/d5/S97nh67cxocvx78TcBiRc8WWPPfzopkkZlmcQJBANLJcwHQZXvq81op
0Xl6K88DPBfxdbsgoAg4fYWWoEqJqWND6L/xeQkXXJrUzea8PeHjOmvP4Z7OA8vz
E1OgILMCQQCMfZy94fSd3usEN05P60Bc9iPGlvqA9NmOiTV7pvxTiyuw2owZLO4j
3jk/xvdFWzn2Ofvf9J4BXgelQk6srr8rAkEAnZEVNX/wvMcPDpFAE0yuPtsuKr/G
wBFNT5fazOeh/tYVFy0GaaU6Uv9xrBPzrs18fMT9QOZuw+VAlU6pXdPPXQJAZgzU
b2WTn52Oj2hxHUJgZWZyBE4lNskkwxHN5L0ear0cBoIp5Bur/Cfu4/HuKdYjW0Ux
PbPdllasLRHmPi9NMwJABxnA1/ppGXB+MZuXbIfUdTm9zxPsAM1HcDkX+41YfBdq
+kG3pr1BjM5SWfz1eJo5E1ACJravP2i+VIqM6IfoZA==
-----END RSA PRIVATE KEY-----";

            Key = (new PemReader(new StringReader(privPem.Trim())).ReadObject() as AsymmetricCipherKeyPair).Private;
            RSAEngine = new RsaEngine();
            RSAEngine.Init(true, Key);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            var dat = Convert.FromBase64String(str);
            var encoding = new Pkcs1Encoding(RSAEngine);
            encoding.Init(false, Key);
            return Encoding.UTF8.GetString(encoding.ProcessBlock(dat, 0, dat.Length));
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            var dat = Encoding.UTF8.GetBytes(str);
            var encoding = new Pkcs1Encoding(RSAEngine);
            encoding.Init(true, Key);
            return Convert.ToBase64String(encoding.ProcessBlock(dat, 0, dat.Length));
        }
    }
}