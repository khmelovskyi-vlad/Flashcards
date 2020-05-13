using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    struct UserTypesOfStudy
    {
        public UserTypesOfStudy(TypesOfStudy TypeOfStudy, TypesOfStudy TypeOfTranslation)
        {
            this.TypeOfStudy = TypeOfStudy;
            this.TypeOfTranslation = TypeOfTranslation;
        }
        public TypesOfStudy TypeOfStudy { get; set; }
        public TypesOfStudy TypeOfTranslation { get; set; }
    }
}
