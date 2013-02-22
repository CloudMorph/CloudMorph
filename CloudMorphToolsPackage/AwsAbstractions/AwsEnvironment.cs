using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CloudAbstractions;

namespace AwsAbstractions
{
    public class AwsEnvironment : IEnvironment
    {
        private string _myLocalAddress;
        private string _myPublicAddress;

        public string MyLocalAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_myLocalAddress))
                {
                    try
                    {
                        _myLocalAddress = Encoding.ASCII.GetString(new WebClient().DownloadData("http://169.254.169.254/latest/meta-data/local-hostname"));
                    }
                    catch (Exception e)
                    {
                        _myLocalAddress = "NaN";
                    }
                }

                return _myLocalAddress;
            }
        }

        public string MyPublicAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_myPublicAddress))
                {
                    try
                    {
                        _myPublicAddress = Encoding.ASCII.GetString(new WebClient().DownloadData("http://169.254.169.254/latest/meta-data/public-hostname"));
                    }
                    catch (Exception e)
                    {
                        _myPublicAddress = "NaN";
                    }
                }

                return _myPublicAddress;
            }
        }

        public IDictionary<string, string> GetMetadata()
        {
            var kv = new Dictionary<string, string>();
            string metadata = "NaN";

            try
            {
                metadata = Encoding.ASCII.GetString(
                    new WebClient().DownloadData("http://169.254.169.254/latest/user-data/"));

                kv["metadata"] = metadata;
            }
            catch
            {
            }

            return kv;
        }
    }
}