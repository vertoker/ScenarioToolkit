namespace Scenario.Editor.SRF
{
    public struct SRFMetadata
    {
        public string FunctionName;
        public string FunctionTooltip;
        public bool Disabled;
        
        public SRFMetadata(string functionName, string functionTooltip = "", bool disabled = false)
        {
            FunctionName = functionName;
            FunctionTooltip = functionTooltip;
            Disabled = disabled;
        }

        public bool ContainsQuery(string searchQuery)
        {
            return (!string.IsNullOrWhiteSpace(FunctionName) && FunctionName.Contains(searchQuery)) 
                   || (!string.IsNullOrWhiteSpace(FunctionTooltip) && FunctionTooltip.Contains(searchQuery));
        }
    }
}