using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web.Models;

namespace web.Controllers
{
    public class validatorAuditoria : AbstractValidator<Auditorias>
    {
        public validatorAuditoria()
        {
            RuleFor(m => m.FechaInicio)
                .NotEmpty()
                .WithMessage("Fecha de inicio requerida")
                .GreaterThanOrEqualTo(m => m.Plan.FechaInicio)
                .WithMessage(m => "La fecha debe ser mayor o igual a " + m.Plan.FechaInicio.ToShortDateString());

            RuleFor(m => m.FechaFin)
                .NotEmpty().WithMessage("Fecha de finalización requerida")
                .GreaterThanOrEqualTo(m => m.FechaInicio)
                                .WithMessage("La fecha de fin debe ser mayor que la de inicio");
        }
    }
}