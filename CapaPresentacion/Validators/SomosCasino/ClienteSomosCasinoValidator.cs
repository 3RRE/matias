using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaNegocio;
using CapaNegocio.AsistenciaCliente;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CapaPresentacion.Validators.SomosCasino {
    public class ClienteSomosCasinoValidator : AbstractValidator<ClienteSomosCasino> {
        private readonly AST_TipoDocumentoBL ast_TipoDocumentoBL;
        private readonly SalaBL salaBL;
        private readonly List<string> GenerosPermitidos;
        private readonly int CodigoEmpresaHolding;

        public ClienteSomosCasinoValidator() {
            ast_TipoDocumentoBL = new AST_TipoDocumentoBL();
            salaBL = new SalaBL();
            GenerosPermitidos = new List<string> { "M", "F" };
            CodigoEmpresaHolding = Convert.ToInt32(ConfigurationManager.AppSettings["CodigoEmpresaHolding"]);
            AddRules();
        }

        private void AddRules() {
            #region Nombres
            RuleFor(x => x.Nombres)
                .NotNull().WithMessage("El nombre es obligatorio.")
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres.");
            #endregion

            #region Apellido Paterno
            RuleFor(x => x.ApellidoPaterno)
                .NotNull().WithMessage("El apellido paterno es obligatorio.")
                .NotEmpty().WithMessage("El apellido paterno es obligatorio.")
                .MinimumLength(2).WithMessage("El apellido paterno debe tener al menos 2 caracteres.")
                .MaximumLength(50).WithMessage("El apellido paterno no debe exceder los 50 caracteres.");
            #endregion

            #region Apellido Materno
            RuleFor(x => x.ApellidoMaterno)
                .NotNull().WithMessage("El apellido materno es obligatorio.")
                .NotEmpty().WithMessage("El apellido materno es obligatorio.")
                .MinimumLength(2).WithMessage("El apellido materno debe tener al menos 2 caracteres.")
                .MaximumLength(50).WithMessage("El apellido materno no debe exceder los 50 caracteres.");
            #endregion

            #region Tipo de Documento
            RuleFor(x => x.IdTipoDocumento)
                .GreaterThan(0).WithMessage("Debe seleccionar un tipo de documento válido.")
                .Must(EsTipoDocumentoValido).WithMessage("Debe seleccionar un tipo de documento válido.");
            #endregion

            #region Número de Documento
            RuleFor(x => x.NumeroDocumento)
                .NotNull().WithMessage("El número de documento es obligatorio.")
                .NotEmpty().WithMessage("El número de documento es obligatorio.")
                .MinimumLength(7).WithMessage("El número de documento debe tener al menos 7 caracteres.")
                .MaximumLength(20).WithMessage("El número de documento no debe exceder los 20 caracteres.");
            #endregion

            #region Fecha de Nacimiento
            RuleFor(x => x.FechaNacimiento)
                .NotNull().WithMessage("La fecha de nacimiento es obligatoria.")
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.")
                .Must(EsFechaValida).WithMessage("La fecha de nacimiento no tiene un formato válido (dd/MM/yyyy).")
                .Must(EsFechaDentroDeRangoSqlServer).WithMessage("La fecha de nacimiento está fuera del rango permitido (01/01/1753 a 31/12/9999).")
                .Must(EsMayorDeEdad).WithMessage("El cliente debe ser mayor de edad.");
            #endregion

            #region Género
            RuleFor(x => x.Genero)
                .NotNull().WithMessage("El género es obligatorio.")
                .NotEmpty().WithMessage("El género es obligatorio.")
                .Must(EsGeneroValido).WithMessage(x => $"El campo 'Genero' no es válido. Los valores permitidos son: {string.Join(", ", GenerosPermitidos)}.");
            #endregion

            #region Celular
            RuleFor(x => x.Celular)
                .NotNull().WithMessage("El celular es obligatorio.")
                .NotEmpty().WithMessage("El celular es obligatorio.")
                .Matches(@"^\d{9,15}$").WithMessage("El celular debe contener entre 9 y 15 dígitos.");
            #endregion

            #region Código de país
            RuleFor(x => x.CodigoPais)
                .NotNull().WithMessage("El código de país es obligatorio.")
                .NotEmpty().WithMessage("El código de país es obligatorio.")
                .Matches(@"^\d{1,4}$").WithMessage("El código de país debe contener solo números (sin '+') y tener entre 1 y 4 dígitos.");
            #endregion

            #region CodSala
            RuleFor(x => x.CodSala)
                .NotNull().WithMessage("Debe especificar una sala válida.")
                .GreaterThan(0).WithMessage("Debe especificar una sala válida.")
                .Must(EsSalaValida).WithMessage(x => $"No existe sala con código '{x.CodSala}'.");
            #endregion

            #region Salas aplica código promocional
            RuleFor(x => x.SalasAplicaCodigoPromocional)
                .NotNull().WithMessage("Tiene que agregar por lo menos una sala en la que el podrá canjear el código promocional.")
                .Must(x => x.Any()).WithMessage("Tiene que agregar por lo menos una sala en la que el podrá canjear el código promocional.")
                .Must(SonSalasValidas).WithMessage("Algunas salas no existen, revisar lista de salas en la que el cliente podrá canjear el código promocional.");
            #endregion

            #region Monto Recargado
            RuleFor(x => x.MontoRecargado)
                .NotNull().WithMessage("El monto recargado debe ser mayor a 0.")
                .GreaterThan(0).WithMessage("El monto recargado debe ser mayor a 0.");
            #endregion
        }

        private bool EsTipoDocumentoValido(int idTipoDocumento) {
            AST_TipoDocumentoEntidad tipoDocumento = ast_TipoDocumentoBL.GetTipoDocumentoID(idTipoDocumento);
            return tipoDocumento.Existe();
        }

        private bool EsSalaValida(int codSala) {
            SalaEntidad sala = salaBL.ObtenerSalaPorCodigo(codSala);
            return sala.Existe();
        }

        private bool EsFechaValida(string date) {
            return DateTime.TryParse(date, out _);
        }

        private bool EsMayorDeEdad(string date) {
            if(!DateTime.TryParse(date, out var birthDate)) return false;
            var age = DateTime.Today.Year - birthDate.Year;
            if(birthDate > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }

        private bool EsGeneroValido(string genero) {
            if(string.IsNullOrWhiteSpace(genero)) {
                return false;
            }
            return GenerosPermitidos.Contains(genero.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        private bool SonSalasValidas(List<int> salasIds) {
            if(salasIds == null || salasIds.Count == 0)
                return false;

            List<int> salasExistentes = salaBL.ListadoTodosSalaActivosOrderJson()
                .Where(x => x.Activo && x.Estado == 1 && x.CodEmpresa == CodigoEmpresaHolding)
                .Select(x => x.CodSala)
                .ToList();
            return salasIds.All(x => salasExistentes.Contains(x));
        }

        private bool EsFechaDentroDeRangoSqlServer(string date) {
            if(!DateTime.TryParse(date, out var parsedDate)) return false;

            DateTime minDate = new DateTime(1753, 1, 1);
            DateTime maxDate = new DateTime(9999, 12, 31);

            return parsedDate >= minDate && parsedDate <= maxDate;
        }
    }
}