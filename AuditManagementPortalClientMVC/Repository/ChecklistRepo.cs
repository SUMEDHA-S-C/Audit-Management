﻿using AuditManagementPortalClientMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuditManagementPortalClientMVC.Repository
{
    public class ChecklistRepo : IChecklistRepo
    {
        public List<string> ProvideChecklist(string audittype)
        {

            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(audittype);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://52.158.168.235/api/AuditChecklist/" + audittype),

                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            List<string> listOfQuestions = new List<string>();
            var response = client.SendAsync(request).ConfigureAwait(false);

            var responseInfo = response.GetAwaiter().GetResult();
            //HttpResponseMessage response = client.GetAsync("http://localhost:14089/api/AuditChecklist" + "/audittype").Result; //client.BaseAddress
            //if (response.IsSuccessStatusCode)
            //{
            //    string data = response.Content.ReadAsStringAsync().Result;
            //    listOfQuestions = JsonConvert.DeserializeObject<List<CQuestions>>(data);
            //}

            if (responseInfo.IsSuccessStatusCode)
            {
                var questions = responseInfo.Content.ReadAsStringAsync().Result;
                listOfQuestions = JsonConvert.DeserializeObject<List<string>>(questions);
            }
            return listOfQuestions;
            //throw new NotImplementedException();
        }
    }
}
