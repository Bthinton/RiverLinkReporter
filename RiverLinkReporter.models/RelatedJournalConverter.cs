using FileHelpers;
using System;
using System.Collections;
using System.Linq;

namespace RiverLinkReporter.models
{

    public class RelatedJournalConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return from.Split(',').Select(x => Convert.ToInt32(x));
        }

        public override string FieldToString(object fieldValue)
        {
            var result = ((IEnumerable)fieldValue).Cast<object>();
            return String.Join(",", result.Select(x => x.ToString()).ToArray());
        }

    }
}