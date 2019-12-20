using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web.Models;

namespace web.Controllers
{
    public class ValidadorActividades: AbstractValidator<Actividades>
    {
        public ValidadorActividades()
        {
            RuleFor(m => m.FechaInicio)
                .NotEmpty()
                .WithMessage("Fecha de inicio requerida")
                .GreaterThanOrEqualTo(m => m.Fase.FechaInicio)
                                .WithMessage(m => "La fecha debe ser mayor o igual a " + m.Fase.FechaInicio.ToShortDateString())
                .LessThanOrEqualTo(m => m.Fase.FechaFin)
                                .WithMessage(m => "La fecha debe menor o igual a " + m.Fase.FechaFin.ToShortDateString());

            RuleFor(m => m.FechaFin)
                .NotEmpty().WithMessage("Fecha de finalización requerida")
                .GreaterThanOrEqualTo(m => m.FechaInicio)
                                .WithMessage("La fecha de fin debe ser mayor que la de inicio")
                .GreaterThanOrEqualTo(m => m.Fase.FechaInicio)
                                .WithMessage(m => "La fecha debe ser mayor o igual a " + m.Fase.FechaInicio.ToShortDateString())
                .LessThanOrEqualTo(m => m.Fase.FechaFin)
                                .WithMessage(m => "La fecha debe menor o igual a " + m.Fase.FechaFin.ToShortDateString())
                .When(m => m.FechaFin != null);
        }
    }
}