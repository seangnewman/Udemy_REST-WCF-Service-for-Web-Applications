﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Udemy_REST_WCF_Service_for_Web_Applications.Service
{
    [ServiceContract(Namespace = "")]     
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]                        // Use a rest call using http Get
        public string DoWork()
        {
            // Add your operation implementation here
            return "Hello from the WCF service!";
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
