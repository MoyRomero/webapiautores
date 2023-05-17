using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Controllers.V1;
using WebApiAutores.Tests.Mocks;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTest
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtenemos4Links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Estado = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();

            //ejecucion
            var resultado = await rootController.Get();


            //verificacion
            Assert.AreEqual(4,resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Estado = AuthorizationResult.Failed();
            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();

            //ejecucion
            var resultado = await rootController.Get();


            //verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2Links_UsandoMoq()
        {
            //preparacion
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            authorizationServiceMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
                                             It.IsAny<object>(),
                                             It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                                             )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            authorizationServiceMock.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
                                 It.IsAny<object>(),
                                 It.IsAny<string>()
                                 )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var UrlHelperMock = new Mock<IUrlHelper>();
            UrlHelperMock.Setup(x => x.Link(
                                            It.IsAny<string>(), 
                                            It.IsAny<object>())).Returns(string.Empty);

            var rootController = new RootController(authorizationServiceMock.Object);
            rootController.Url = UrlHelperMock.Object;

            //ejecucion
            var resultado = await rootController.Get();


            //verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}
