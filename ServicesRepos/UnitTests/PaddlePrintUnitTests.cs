using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Contrib.HttpClient;
using Shared.Kernel.Enums;
using Shared.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class PaddlePrintUnitTests
    {
        private Mock<IRandomTripService> _mockService = new Mock<IRandomTripService>();
        private Mock<IScheduleDBRepo> _mockRepo = new Mock<IScheduleDBRepo>();

        public string BaseUrl = "http://localhost:7071";
        Mock<HttpMessageHandler> mock = new Mock<HttpMessageHandler>();

     [TestInitialize]
        public void Initialize()
        {
            //init what the get routes function returns
            IEnumerable<IRouteModel> routes = new List<Mock_RouteModel> {
                new Mock_RouteModel { RouteId = "1" },
                new Mock_RouteModel {  RouteId = "5" }
            };
            //_mockRepo.Setup(x => x.GetRoutes()).Returns(Task.FromResult(routes));
            _mockRepo.Setup(x => x.GetRoutes()).Returns(Task.FromResult(routes));

            IEnumerable<IPaddlePrintModel> paddles = new List<Mock_PaddlePrintModel> {
                new Mock_PaddlePrintModel { RouteID = 1 },
                new Mock_PaddlePrintModel {  RouteID = 5 }
            };
            IPaddlePrintModel mockPaddle = new Mock_PaddlePrintModel();
            //mock what paddleprint repo call returns;
           // _mockRepo.Setup(x =>
            //x.GetPaddlePrint(PickType.NextPick, DayOfWeek.Sunday, ""))
            //    .Returns(Task.FromResult(paddles));



            //  var result = _mockService.Setup(x=>x.get)

            //TapApprovalService = new TapApprovalService(ServiceLocator.GetInstance<ITapApprovalRepo>());
        }
       
        [TestMethod]
        public async Task TestMethod1()
        {
            
            Assert.IsTrue(true, "Queue failed.");

        }
    }

}