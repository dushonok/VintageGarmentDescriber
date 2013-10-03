using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageGarmentDescriber
{
    public class GarmentDescription
    {
        public GarmentDescription()
        {
            descriptions = new List<String>(50);
            descriptions.Add("Vintage");
            ++idx;

            measurements = new Dictionary<string,string>();
            garmentSynonims = new Dictionary<string, string>();

            FillDefinitions();

        }

        private void FillDefinitions()
        {
            measurements.Clear();
            garmentSynonims.Clear();

            measurements.Add("blouse", String.Join("\n",
                                    "Size: ",
                                    "Fit: ",
                                    "",
                                    "Length: ''",
                                    "Shoulder: ''",
                                    "Sleeves: ''",
                                    "Bust:''",
                                    "Waist: ''"));
            garmentSynonims.Add("blouse", String.Join("\n",
                                    "shirt"
                                ));

            measurements.Add("jacket", measurements["blouse"]);
            garmentSynonims.Add("jacket", String.Join("\n",
                                    "blaser"
                                ));

            measurements.Add("coat", measurements["blouse"]);

            measurements.Add("dress", measurements["blouse"] + "\n" +
                                    String.Join("\n",
                                    "Belt: ''"));

            measurements.Add("skirt", String.Join("\n",
                                    "Size: ",
                                    "Fit: ",
                                    "",
                                    "Length (by side seam): ''",
                                    "Waist: ''",
                                    "Hips: ''"
                                ));
            measurements.Add("shorts", measurements["skirt"] + "\n" +
                                String.Join("\n",
                                    "Rise: ''",
                                    "Inseam: ''",
                                    "Inseam: ''",
                                    "Flare: ''"
                                    ));
            measurements.Add("pants", measurements["shorts"]);
            garmentSynonims.Add("pants", String.Join("\n",
                                    "trousers"
                                ));

            measurements.Add("jumper", measurements["shorts"]);
            measurements["jumper"] = measurements["jumper"] + "\n" +
                                    String.Join("\n",
                                    "Shoulder: ''",
                                    "Bust: ''"
                                    );
            garmentSynonims.Add("jumper", String.Join("\n",
                                    "romper",
                                    "overall"
                                ));

            measurements.Add("purse", String.Join("\n",
                                    "Bag Depth: ''",
                                    "Strap Drop: ''",
                                    "Handle Drop: ''",
                                    "Bag Width: ''",
                                    "Bag Length: ''",
                                    "Bag Height: ''"));
        }

        String UsageType { get { return GetField(0); } } // vintage / second hand
        String Type { get { return GetField(1); } } // dress /pants / etc
        String Year { get { return GetField(2); } } // 1960s / 1970s / etc
        String Material 
        { 
            get 
            {
                String material = GetField(3).ToLower();
                return material.Equals("n/a") ? "" : material; 
            } 
        }
        String SizeOfModel { get { return ""; } } // Nadya/ Michelle/Myriam/Maude
        String Sleeve
        {
            get
            {
                String sleeve = GetField(4).ToLower();
                return sleeve.Equals("n/a") ? "" : sleeve;
            }
        }
        String SleevePlural { get { return String.IsNullOrEmpty(Sleeve) ? "" : (Sleeve + "s"); } }


        Dictionary<String, String> measurements;
        Dictionary<String, String> garmentSynonims;

        String GetField(Int32 idx)
        {
            if (descriptions.Count <= idx)
                return "";
            return descriptions[idx].ToLower();
        }

        public Int32 ColNumber
        {
            get
            {
                return descriptions.Count;
            }
        }

        public String ConvertToFileLine()
        {
            if (String.IsNullOrEmpty(Type))
                return "";

            String res = String.Join("\t", 
                GetShortTitle(), 
                GetFullTitle(), 
                GetDesc(),
                GetCondition(),
                GetIncludedAccessories(),
                GetModelSize(),
                GetMeasurements(),
                "",
                "",
                GetTags(),
                Material
            );

            return res;
        }


        public void Add(Int32 newIdx, String txt)
        {
            ++newIdx;

            while (descriptions.Count <= newIdx)
            {
                descriptions.Add("");
            }

            descriptions[newIdx] = txt;
        }

        public void GotoToIdxAndChange(Int32 newIdx, String txt)
        {
            idx = newIdx;
            descriptions[newIdx] = txt;
        }

        String GetFullTitle()
        {
            String title = String.Join(" ", 
                    WordFirstLettersToUpper(UsageType), 
                    WordFirstLettersToUpper(Year),
                    WordFirstLettersToUpper(Sleeve), 
                    WordFirstLettersToUpper(Type)
                  ).Trim();
            title.Replace('\'', '\x0');
            return PutInQuotes(title);
        }

        String GetShortTitle()
        {
            return PutInQuotes(String.Join(" ", Year, Sleeve, Type).Trim());
        }

        String GetDesc()
        {
            String newUsageType = WordFirstLettersToUpper(UsageType);
            String newType = AllToLower(Type);
            String madeIn = String.IsNullOrEmpty(Year) ? "" : "made in " + Year;
            String desc = String.Join(" ", newUsageType, newType, madeIn);
            desc = String.Join("\n", 
                    desc, 
                    VeryFirstLetterToUpper(SleevePlural.ToLower()), 
                    VeryFirstLetterToUpper(Material.ToLower())
                );
            return PutInQuotes(desc.Trim());
        }

        String GetCondition()
        {
            return PutInQuotes("Great vintage condition");
        }

        String GetIncludedAccessories()
        {
            return PutInQuotes("None of the accessories or other items included");
        }

        String GetMeasurements()
        {
            if (!measurements.ContainsKey(Type.ToLower()))
                return "";
            return PutInQuotes(measurements[Type.ToLower()]);
        }

        String GetTags()
        {
            String tags = String.Join(",", UsageType, Type, Year, Material, SleevePlural);
            if (garmentSynonims.ContainsKey(Type.ToLower()))
                tags = String.Join(",", tags, garmentSynonims[Type.ToLower()]);
            return PutInQuotes(tags);
        }

        String GetModelSize()
        {
            return PutInQuotes(" ");
        }

        String PutInQuotes(String txt)
        {
            txt = '"' + txt + '"';
            return txt;
        }

        String AllToLower(String txt)
        {
            if (String.IsNullOrEmpty(txt))
                return "";
           
            return txt.ToLower();
        }

        String WordFirstLettersToUpper(String txt)
        {
            if (String.IsNullOrEmpty(txt))
                return "";

            String[] words = txt.Split(' ');
            for (int i = 0; i < words.Length; ++i )
            {
                String str = words[i].Trim();
                words[i] = str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
            }

            return String.Join(" ", words);
        }

        String VeryFirstLetterToUpper(String str)
        {
            if (String.IsNullOrEmpty(str))
                return "";

            return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
        }

        List<String> descriptions;
        Int32 _idx;
        Int32 idx
        {
            get { return _idx; }
            set
            {
                _idx = value;
                if (value < 0)
                    _idx = 0;
            }
        }

    }
}
