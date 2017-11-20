using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace SIPCA.CLASES.Models
{
    public class Unique : ValidationAttribute
    {
         SIPCA.CLASES.Context.ModelContext db = new SIPCA.CLASES.Context.ModelContext();
       
        private readonly string table;
        private readonly string propiedad;
        public Unique( string _table, string _propiedad): base( "{0} Ya existe")
        {
            table = _table;
            propiedad = _propiedad;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                Debug.WriteLine("nombre tabla " + table);
                switch (table)
                {
                    case "Categoria":
                        {
                            var categorias = db.Categorias.ToList();
                            foreach (Categoria cat in categorias )
                            {
                                if(cat.Nombre.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))
                                {
                                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                                    return new ValidationResult(errorMessage);
                                }
                            }
                            break;
                        }
       
                    
                }
          
            }
            return ValidationResult.Success;
        }
    }
}