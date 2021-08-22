namespace DnaFragment.Data
{
    public class DataConstants
    {
        public class DefaultConstants
        {
            public const int IdMaxLength = 40;
            public const int DefaultMaxLength = 20;
            public const int DefaultMinLength = 3;
            public const int IssueDescriptionMinLength = 5;

            public const int DefaultDescriptionMinLength = 10;
            public const int DefaultDescriptionMaxLength = 140;
        }


        public class LrProductConst
        {
            public const int ModelMinLength = 5;
            public const int MaxVolume = 5;
            public const int MinVolume = 2;
            public const int ModelMaxLength = 70;
            public const int DescriptionMinLength = 20;
            public const int DescriptionMaxLength = 800;
            public const int ChemicalDescriptionMaxLength = 250;
            public const int PlateNumberMaxLength = 7;
            public const string PlateNumberRegularExpression = @"^[0-9]+$";
            public const int YearMinValue = 1900;
            public const int YearMaxValue = 2100;
        }

        public class Category
        {
            public const int NameMaxLength = 25;
        }

        public class LrUserConst
        {
            public const int MinLengthFullName = 5;
            public const int MaxLengthFullName = 100;
            public const int MinLengthPassword = 5;
            public const int MaxLengthPassword = 50;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;
            public const int CodeNumber = 999999;
        }   
    }
}



