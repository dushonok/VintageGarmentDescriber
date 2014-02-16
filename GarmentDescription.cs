using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VintageGarmentDescriber
{
    public class GarmentDescription
    {
        const String tagSeparator = ", ";

        public GarmentDescription()
        {
            descriptions = new List<String>(50);
            descriptions.Add("Vintage");
            ++idx;

            measurements = new Dictionary<string,string>();
            synonims = new Dictionary<string, string>();

            FillDefinitions();

        }

        private void FillDefinitions()
        {
            measurements.Clear();
            synonims.Clear();

            measurements.Add("blouse", String.Join("\n",
                                    "Size: ",
                                    "Fit: ",
                                    "",
                                    "Length: ''",
                                    "Shoulder: ''",
                                    "Sleeves: ''",
                                    "Bust:''",
                                    "Waist: ''"));
            synonims.Add("blouse", String.Join("\n",
                                    "shirt"
                                ));

            measurements.Add("jacket", measurements["blouse"]);
            synonims.Add("jacket", String.Join("\n",
                                    "blazer"
                                ));

            measurements.Add("coat", measurements["blouse"]);

            measurements.Add("cape", String.Join("\n",
                                    "Size: ",
                                    "Fit: ",
                                    "",
                                    "Length: ''",
                                    "Shoulder: ''"));
            synonims.Add("cape", String.Join("\n",
                                    "coat",
                                    "cloak",
                                    "sleeveless coat"
                                ));

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
                                    "Flare: ''"
                                    ));
            measurements.Add("pants", measurements["shorts"]);
            synonims.Add("pants", String.Join("\n",
                                    "trousers",
                                    "slacks"
                                ));

            measurements.Add("jumper", measurements["shorts"]);
            measurements["jumper"] = measurements["jumper"] + "\n" +
                                    String.Join("\n",
                                    "Shoulder: ''",
                                    "Bust: ''"
                                    );
            synonims.Add("jumper", String.Join("\n",
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

            synonims.Add("long", String.Join("\n",
                                    "maxi"
                                ));
            synonims.Add("mid calf", String.Join("\n",
                                    "midi"
                                ));
            synonims.Add("short", String.Join("\n",
                                    "mini"
                                ));
            synonims.Add("canada", String.Join("\n",
                                    "Canadian"
                                ));
            synonims.Add("usa", String.Join("\n",
                                    "American"
                                ));
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
        String SleevePlural { 
            get {

                if (String.Compare("Sleeveless", Sleeve, true) == 0)
                    return Sleeve;
                return String.IsNullOrEmpty(Sleeve) ? "" : (Sleeve + "s"); 
            }
        }
        String SkirtLenght {
            get {
                    String lenght = GetField(5).ToLower();
                    return lenght.Equals("n/a") ? "" : lenght; 
                }
        }
        String MadeIn
        {
            get
            {
                String madeIn = GetField(6).ToLower();
                return madeIn.Equals("n/a") ? "" : madeIn;
            }
        }
        String MadeInCountry
        {
            get {
                return String.IsNullOrEmpty(MadeIn) ? "" : "Made in " + VeryFirstLetterToUpper(MadeIn);
            }
        }



        Dictionary<String, String> measurements;
        Dictionary<String, String> synonims;

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
                "", // image
                "", // image link
                GetFullTitle(), 
                GetDesc(),
                GetCondition(),
                GetIncludedAccessories(),
                GetModelSize(),
                GetMeasurements(),
                GetTags(),
                Material,
                "" // price
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
                    WordFirstLettersToUpper(SkirtLenght), 
                    WordFirstLettersToUpper(Type)
                  );
            title.Replace('\'', '\x0');
            return PutInQuotes(RemoveDuplicatedThings(title));
        }

        String GetShortTitle()
        {
            return PutInQuotes(RemoveDuplicatedThings(String.Join(" ", Year, Sleeve, Type)));
        }

        String GetDesc()
        {
            String newUsageType = WordFirstLettersToUpper(UsageType);
            String newType = AllToLower(Type);
            String madeIn = String.IsNullOrEmpty(Year) ? "" : "made in " + Year;
            String desc = String.Join(" ", newUsageType, newType, madeIn);
            desc = String.Join("\n", 
                    desc, 
                    String.IsNullOrEmpty(SkirtLenght) ? "" : VeryFirstLetterToUpper(SkirtLenght) + " skirt",
                    VeryFirstLetterToUpper(SleevePlural.ToLower()), 
                    VeryFirstLetterToUpper(Material.ToLower()),
                    " ",
                    MadeInCountry
                );
            return PutInQuotes(RemoveDuplicatedThings(desc));
        }

        String GetCondition()
        {
            return PutInQuotes("Great vintage condition");
        }

        String GetIncludedAccessories()
        {
            return PutInQuotes("None of the accessories or other items included. Available for purchase in the physical store");
        }

        String GetMeasurements()
        {
            if (!measurements.ContainsKey(Type.ToLower()))
                return "";
            return PutInQuotes(String.Join("\n", 
                    measurements[Type.ToLower()], 
                    " " , 
                    "Wondering how we measure our items? See in our Shop Policies!")
                 );
        }

        String GetTags()
        {
            String tags = String.Join(tagSeparator, UsageType, Type, Year, Material, SleevePlural, SkirtLenght,
                MadeInCountry);
            if (synonims.ContainsKey(Type.ToLower()))
                tags = String.Join(tagSeparator, tags, synonims[Type.ToLower()]);
            if (synonims.ContainsKey(SkirtLenght.ToLower()))
                tags = String.Join(tagSeparator, tags, synonims[SkirtLenght.ToLower()]);
            if (synonims.ContainsKey(Sleeve.ToLower()))
                tags = String.Join(tagSeparator, tags, synonims[Sleeve.ToLower()]);
            if (synonims.ContainsKey(MadeIn.ToLower()))
                tags = String.Join(tagSeparator, tags, synonims[MadeIn.ToLower()]);
            return PutInQuotes(RemoveDuplicatedThings(tags));
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

        String RemoveDuplicatedThings(String input)
        {
            String pattern = "[^\\S\\n]+";
            String replacement = " ";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);

            pattern = "(\\n){2,}";
            replacement = "\n";
            rgx = new Regex(pattern);
            result = rgx.Replace(result, replacement);

            pattern = "(" + tagSeparator + ")+";
            replacement = tagSeparator;
            rgx = new Regex(pattern);
            result = rgx.Replace(result, replacement);

            return result.Trim();
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
