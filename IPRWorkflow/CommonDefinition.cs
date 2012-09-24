using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR
{
  internal class CommonDefinition
  {
    //TODO  [pr4-3550] Add localization to the application http://itrserver/Bugs/BugDetail.aspx?bid=3550
    internal const string SADDocumentLibrary = "SAD Document Library";
    internal const string GoodsDescriptionTobaccoNamePattern = @"\b(.*)(?=\sGRADE:)";
    internal const string GoodsDescriptionWGRADEPattern = @"(?<=\WGRADE:)\W*\b(\w*)";
    internal const string GoodsDescriptionSKUPattern = @"(?<=\WSKU:)\W*\b(\d*)";
    internal const string GoodsDescriptionBatchPattern = @"(?<=\WBatch:)\W*\b(\d*)";
    internal const double WeightTolerance = 0.005;
  }
}
