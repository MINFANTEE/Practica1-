using System.Collections.Generic;
using System.Security.Claims;
using FinancialApp.Web.Controllers;
using FinancialApp.Web.Models;
using FinancialApp.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FinancialApp.Tests.Controllers;

public class CuentaControllerTest
{
    [Test]
    public void CreateViewCase01()
    {
        var mockTipoRepositorio = new Mock<ITipoCuentaRepositorio>();
        mockTipoRepositorio.Setup(o => o.ObtenerTodos()).Returns(new List<TipoCuenta>());
        
        var controller = new CuentaController(mockTipoRepositorio.Object, null, null);
        var view = controller.Create();
        
        Assert.IsNotNull(view);
    }
    
    [Test]
    public void EditViewCase01()
    { 
        var mockTipoRepositorio = new Mock<ITipoCuentaRepositorio>();
        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>();
        mockCuentaRepositorio.Setup(o => o.ObtenerCuentaPorId(2)).Returns(new Cuenta{Id = 1, Nombre = "Joel", Monto = 25});
        var controller = new CuentaController(mockTipoRepositorio.Object,mockCuentaRepositorio.Object, null);
        var view = (ViewResult)controller.Edit(2);
        
        Assert.IsNotNull(view.Model);
        Assert.IsNotNull(view);
       
    }

    [Test]
    public void IndexViewCase01()
    {
        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { 
            new Claim(ClaimTypes.Name, "admin") 
        });

        var mockContext = new Mock<HttpContext>();   
        mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

        var mockCuentaRepo = new Mock<ICuentaRepositorio>();
        mockCuentaRepo.Setup(o => o.ObtenerCuentasDeUsuario(1)).Returns(new List<Cuenta> { 
            new Cuenta()    
        });

        var controller = new CuentaController(null, mockCuentaRepo.Object, null);
        controller.ControllerContext = new ControllerContext() { 
            HttpContext = mockContext.Object 
        };
        var view = (ViewResult)controller.Index();
        Assert.IsNotNull(view);
        Assert.AreEqual(1, ((List<Cuenta>)view.Model).Count);
    }

    [Test]

    public void pruebaCreateCorrecto()
    {

        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>(); // 6
        mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { 
            new Claim(ClaimTypes.Name, "admin") 
        });
        
        var mockContext = new Mock<HttpContext>(); // 5   
        mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>(); //7
        
        var controller = new CuentaController(null,mockCuentaRepositorio.Object,null); // 1
        
        controller.ControllerContext = new ControllerContext() { // 4
            HttpContext = mockContext.Object 
        };
        
        var resultadoc= controller.Create(new Cuenta(){TipoCuentaId = 2}); // 2 (1)llamamos al metodo que deseamos testear (2)Guardar en una variable 

        Assert.IsNotNull(resultadoc); // 3
        
        Assert.IsInstanceOf<RedirectToActionResult>(resultadoc);
        
    }
    
    [Test]
    public void pruebaCreatEerror()
    {

        var mockClaimsPrincipal = new Mock<ClaimsPrincipal>(); // 6
        mockClaimsPrincipal.Setup(o => o.Claims).Returns(new List<Claim> { 
            new Claim(ClaimTypes.Name, "admin") 
        });
        
        var mockContext = new Mock<HttpContext>(); // 5   
        mockContext.Setup(o => o.User).Returns(mockClaimsPrincipal.Object);

        var mockCuentaRepositorio = new Mock<ICuentaRepositorio>(); //7
        var mockTipoCuentaRepositorio = new Mock<ITipoCuentaRepositorio>(); // 8
        
        var controller = new CuentaController(mockTipoCuentaRepositorio.Object,mockCuentaRepositorio.Object,null); // 1
        
        controller.ControllerContext = new ControllerContext() { // 4
            HttpContext = mockContext.Object 
        };
        
        var resultadoc= controller.Create(new Cuenta(){TipoCuentaId = 7}); // 2 (1)llamamos al metodo que deseamos testear (2)Guardar en una variable 

        Assert.IsNotNull(resultadoc); // 3
        
        Assert.IsInstanceOf<ViewResult>(resultadoc);
        
    }
    
}