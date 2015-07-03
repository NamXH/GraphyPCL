using System;

namespace GraphyPCL
{
    // E.g: CustomTagAverageActive = CustomTagTotal / ActiveContactTotal
    public class UserDataViewModel
    {
        public UserData UserData { get; set; }


        public int ContactTotal { get; set; }

        public int ActiveContactTotal { get; set; }


        public int CustomTagTotal { get; set; }

        public double CustomTagAverage { get; set; }

        public double CustomTagAverageActive { get; set; }

        public double CustomTagStandardDeviation { get; set; }

        public double CustomTagStarndardDeviationActive { get; set; }


        public int AutoAddedTagTotal { get; set; }

        public double AutoAddedTagAverage { get; set; }

        public double AutoAddedTagAverageActive { get; set; }

        public double AutoAddedTagStandardDeviation { get; set; }

        public double AutoAddedTagStandardDeviationActive { get; set; }


        public int TagTotal { get; set; }

        public double TagAverage { get; set; }

        public double TagAverageActive { get; set; }

        public double TagStandardDeviation { get; set; }

        public double TagStandardDeviationActive { get; set; }


        public double CustomTagWeightAverage { get; set; }

        public double CustomTagWeightAverageActive { get; set; }

        public double CustomTagWeightStandardDeviation { get; set; }

        public double CustomTagWeightStandardDeviationActive { get; set; }


        public double AutoAddedTagWeightAverage { get; set; }

        public double AutoAddedTagWeightAverageActive { get; set; }

        public double AutoAddedTagWeightStandardDeviation { get; set; }

        public double AutoAddedTagWeightStandardDeviationActive { get; set; }


        public double TagWeightAverage { get; set; }

        public double TagWeightAverageActive { get; set; }

        public double TagWeightStandardDeviation { get; set; }

        public double TagWeightStandardDeviationActive { get; set; }


        public int RelationshipTotal { get; set; }

        public double RelationshipAverage { get; set; }

        public double RelationshipAverageActive { get; set; }

        public double RelationshipStandardDeviation { get; set; }

        public double RelationshipStandardDeviationActive { get; set; }


        public double RelationshipWeightAverage { get; set; }

        public double RelationshipWeightAverageActive { get; set; }

        public double RelationshipWeightStandardDeviation { get; set; }

        public double RelationshipWeightStandardDeviationActive { get; set; }


        public UserDataViewModel()
        {
            UserData = UserDataManager.UserData;
        }
    }
}