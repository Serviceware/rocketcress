using System.Text.RegularExpressions;

namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing;

internal class RegularExpressions
{
    public static Regex SplitPartsRegex { get; } = new Regex(@"
        (?<Path> ([\.\/\|_\<\>]|\{\-?[0-9]+\})*)
        (?<ControlType>([a-zA-Z]+[a-zA-Z0-9\-]* | \*))?
        (\[(?=([^0-9])) (?<Condition>(?>
            (?(STRD) \""(?<-STRD>) | \""(?<STRD>)) |
            (?(STRS) \'(?<-STRS>) | \'(?<STRS>)) | 
            (?(STRS) \[ | (?(STRD) \[ | \[(?<DEPTH>))) | 
            (?(STRS) \] | (?(STRD) \] | \](?<-DEPTH>))) | 
            [^\[\]\'\""]+)*) \] (?(DEPTH)(?!)))? 
        (\[ (?<Skip>[0-9]+) \])?", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    public static Regex SplitPartPathRegex { get; } = new Regex(@"
        (?<Path> [\.\/_\<\>]*)
        (\{ (?<MaxDepth> \-?[0-9]+) \})?", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    public static Regex SplitConditionsRegex { get; } = new Regex(@"
        (?> (?(STRD) \""(?<-STRD>) | \""(?<STRD>)) |
            (?(STRS) \'(?<-STRS>) | \'(?<STRS>)) | 
            (?(STRS) (\[|\() | (?(STRD) (\[|\() | (\[|\()(?<DEPTH>))) | 
            (?(STRS) (\]|\)) | (?(STRD) (\]|\)) | (\]|\))(?<-DEPTH>))) | 
            (?(DEPTH) \s | (?(STRS) \s | (?(STRD) \s | (?!)))) |
            [^\[\]\(\)\'\""\s]+)+ (?(DEPTH)(?!))+", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

    public static Regex PropertyConditionRegex { get; } = new Regex(@"\A
        @(?<Property> [a-zA-Z]+[a-zA-Z0-9\-]*) [\~\=]{1,2} 
        (([\'\""](?<Value>.*)[\'\""])|(?<Value>[^\'\""]*))\Z", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

    public static Regex FunctionConditionRegex { get; } = new Regex(@"\A
        (?<Name>[^\(]+)\(
        (?<Parameters>(?> 
            (?(STRD) \""(?<-STRD>) | \""(?<STRD>)) |
            (?(STRS) \'(?<-STRS>) | \'(?<STRS>)) | 
            (?(STRS) \( | (?(STRD) \( | \((?<DEPTH>))) | 
            (?(STRS) \) | (?(STRD) \) | \)(?<-DEPTH>))) | 
            [^\(\)\'\""]+)*) (?(DEPTH)(?!))?
        \)\Z", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    public static Regex FunctionConditionSplitParametersRegex { get; } = new Regex(@"
        (?> (?(STRD) \""(?<-STRD>) | \""(?<STRD>)) |
            (?(STRS) \'(?<-STRS>) | \'(?<STRS>)) | 
            (?(STRS) (\(|\[) | (?(STRD) (\(|\[) | (\(|\[)(?<DEPTH>))) | 
            (?(STRS) (\)|\]) | (?(STRD) (\)|\]) | (\)|\])(?<-DEPTH>))) | 
            (?(DEPTH) \, | (?(STRS) \, | (?(STRD) \, | (?!)))) |
            [^\(\)\[\]\'\""\,]+)+", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

    public static Regex SearchDescriptionElementRegex { get; } = new Regex(@"\A@[a-zA-Z]+[a-zA-z0-9]*\Z", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    public static Regex SearchDescriptionStringRegex { get; } = new Regex(@"\A(\' [^\']* \' | \"" [^\""]* \"")\Z", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
}
