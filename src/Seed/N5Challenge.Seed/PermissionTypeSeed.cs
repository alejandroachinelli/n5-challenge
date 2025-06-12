using N5Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Seed;

public class PermissionTypeSeed
{
    public static List<PermissionTypeEntity> InitialData()
    {
        return new List<PermissionTypeEntity>
            {
                new PermissionTypeEntity() { Description = "Administrador" }, //ver, editar, eliminar, gestionar, ver registro de actividad de datos
                new PermissionTypeEntity() { Description = "Supervisor" }, // ver, editar, eliminar datos
                new PermissionTypeEntity() { Description = "Operador" }, // ver, editar datos
                new PermissionTypeEntity() { Description = "Auditor" }, // ver, ver registro de actividad de datos
                new PermissionTypeEntity() { Description = "Cliente" }, // ver datos
            };
    }
}
