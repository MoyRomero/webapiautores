using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PimeraLetraMinusculaDaError()
        {
            //preparacion
            var primeraLetra = new FirstLetterUCAttribute();
            var valor = "moises";
            var valContext = new ValidationContext(new {Nombre = valor});

            //ejecucion
            var resultado = primeraLetra.GetValidationResult(valor,valContext);

            //verificacion
            Assert.AreEqual("La primera letra del campo, debe ser mayúscula.", resultado.ErrorMessage);
        }

        [TestMethod]
        public void ValorNuloNoDaError()
        {
            //preparacion
            var primeraLetra = new FirstLetterUCAttribute();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //ejecucion
            var resultado = primeraLetra.GetValidationResult(valor, valContext);

            //verificacion
            Assert.IsNull(resultado);
        }
    }
}