﻿using Microsoft.CSS.Core;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;

namespace MadsKristensen.EditorExtensions
{
    [Export(typeof(ICssItemChecker))]
    [Name("StarSelectorErrorTagProvider")]
    [Order(After = "Default Declaration")]
    internal class StarSelectorErrorTagProvider : ICssItemChecker
    {
        public ItemCheckResult CheckItem(ParseItem item, ICssCheckerContext context)
        {
            SimpleSelector sel = (SimpleSelector)item;

            if (!WESettings.GetBoolean(WESettings.Keys.ValidateStarSelector) || !sel.IsValid || context == null)
                return ItemCheckResult.Continue;

            if (sel.Text == "*")
            {
                    string errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.PerformanceDontUseStarSelector);

                    SimpleErrorTag tag = new SimpleErrorTag(sel, errorMessage);

                    context.AddError(tag);
            }

            return ItemCheckResult.Continue;
        }


        public IEnumerable<Type> ItemTypes
        {
            get { return new[] { typeof(SimpleSelector) }; }
        }
    }
}
