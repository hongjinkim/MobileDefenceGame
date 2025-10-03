namespace GoogleSheet.Type
{
    [Type(Type: typeof(bool), TypeName: new string[] { "bool", "boolean", "Bool" })]
    public class BoolType : IType
    {
        public object DefaultValue => false;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

     
            if(value.ToLower() == "true" || value == "1")
                return true;
            else if (value.ToLower() == "false" || value == "0")
                return false;
            else
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
 
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
