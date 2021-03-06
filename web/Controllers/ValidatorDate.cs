﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web.Models;

namespace web.Controllers
{
    public class ValidatorDate : AbstractValidator<Fases>
    {
        public ValidatorDate()
        {
            RuleFor(m => m.FechaInicio)
                .NotEmpty()
                .WithMessage("Fecha de inicio requerida")
                .GreaterThanOrEqualTo(m => m.Auditoria.FechaInicio)
                                .WithMessage(m => "La fecha debe ser mayor o igual a " + m.Auditoria.FechaInicio.ToShortDateString())
                .LessThanOrEqualTo(m => m.Auditoria.FechaFin)
                                .WithMessage(m => "La fecha debe menor o igual a " + m.Auditoria.FechaFin.ToShortDateString());

            RuleFor(m => m.FechaFin)
                .NotEmpty().WithMessage("Fecha de finalización requerida")
                .GreaterThanOrEqualTo(m => m.FechaInicio)
                                .WithMessage("La fecha de fin debe ser mayor que la de inicio")
                .GreaterThanOrEqualTo(m=>m.Auditoria.FechaInicio)
                                .WithMessage(m=>"La fecha debe ser mayor o igual a " + m.Auditoria.FechaInicio.ToShortDateString())
                .LessThanOrEqualTo(m=>m.Auditoria.FechaFin)
                                .WithMessage(m=>"La fecha debe menor o igual a " + m.Auditoria.FechaFin.ToShortDateString())
                .When(m => m.FechaFin!=null);
        }
    }
}