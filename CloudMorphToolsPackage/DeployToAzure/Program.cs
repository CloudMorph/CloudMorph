﻿using System;
using AzureAbstractions;
using CloudAbstractions;
using CloudAbstractions.Messaging;

namespace DeployToAzure
{
    class Program
    {
        private static IRealm _realm;
        private static IQueueProvider _queueProvider;

        static void Main(string[] args)
        {
            try
            {
                _realm = new AzureRealm();
                _queueProvider = _realm.QueueProvider;

                const string queueId = "cloudmorph-testqueue";
                try
                {
                    var queue = _queueProvider.GetQueueById(queueId);
                    Console.WriteLine("Queue exists.");
                    return;
                }
                catch
                {
                }
                bool bResult = _queueProvider.CreateQueue(queueId);

                if (bResult)
                    Console.WriteLine("The queue {0} was successfully created", queueId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
