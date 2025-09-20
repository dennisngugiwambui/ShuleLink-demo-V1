using ShuleLink.Models;

namespace ShuleLink.Services
{
    public class LearningContentService
    {
        public List<LearningTopic> GetComprehensiveTopics(string grade)
        {
            var topics = new List<LearningTopic>();

            switch (grade)
            {
                case "1":
                case "2":
                case "3":
                    topics.AddRange(GetEarlyGradeTopics());
                    break;
                case "4":
                case "5":
                    topics.AddRange(GetMiddleGradeTopics());
                    break;
                case "6":
                case "7":
                    topics.AddRange(GetUpperGradeTopics());
                    break;
            }

            return topics;
        }

        private List<LearningTopic> GetEarlyGradeTopics()
        {
            return new List<LearningTopic>
            {
                new LearningTopic
                {
                    Title = "Animals and Their Homes",
                    Subject = "Science",
                    Grade = "1-3",
                    Icon = "🐾",
                    Color = "#27AE60",
                    Content = @"# Animals and Their Homes

## Wild Animals
- **Lions** live in dens in the savanna
- **Birds** build nests in trees
- **Fish** live in water (rivers, lakes, oceans)
- **Monkeys** live in trees in forests
- **Elephants** roam in herds across grasslands

## Domestic Animals
- **Dogs** live in kennels or houses with families
- **Cats** live in homes and like cozy spaces
- **Cows** live in barns on farms
- **Chickens** live in coops
- **Horses** live in stables

## Fun Facts
- Bees live in hives and make honey
- Ants live in underground colonies
- Bears hibernate in caves during winter
- Penguins live in cold places like Antarctica

## Activity
Draw your favorite animal and its home!"
                },
                new LearningTopic
                {
                    Title = "My Family",
                    Subject = "Social Studies", 
                    Grade = "1-3",
                    Icon = "👨‍👩‍👧‍👦",
                    Color = "#3498DB",
                    Content = @"# My Family

## Family Members
- **Father/Dad** - The male parent who takes care of the family
- **Mother/Mom** - The female parent who loves and cares for children
- **Brother** - A male child in the family
- **Sister** - A female child in the family
- **Grandparents** - Mom and Dad's parents

## Family Roles
- Parents work to provide food, shelter, and education
- Children help with simple chores and study hard
- Everyone shows love and respect to each other
- Families eat meals together and share stories

## Types of Families
- **Nuclear family** - Parents and children
- **Extended family** - Includes grandparents, aunts, uncles
- **Single parent family** - One parent with children

## Family Values
- Love and care for each other
- Help when someone is sick or sad
- Share toys and food
- Respect elders
- Work together as a team"
                }
            };
        }

        private List<LearningTopic> GetMiddleGradeTopics()
        {
            return new List<LearningTopic>
            {
                new LearningTopic
                {
                    Title = "The Human Body Systems",
                    Subject = "Science",
                    Grade = "4-5", 
                    Icon = "🫀",
                    Color = "#E74C3C",
                    Content = @"# The Human Body Systems

## Respiratory System
The respiratory system helps us breathe and get oxygen.

### Parts:
- **Nose/Mouth** - Air enters here
- **Trachea (Windpipe)** - Tube that carries air to lungs
- **Lungs** - Two organs that take in oxygen
- **Diaphragm** - Muscle that helps us breathe

### How it works:
1. We breathe in air through nose/mouth
2. Air travels down the trachea
3. Lungs take oxygen from air
4. Oxygen goes to blood
5. We breathe out carbon dioxide

## Digestive System
Breaks down food so our body can use it for energy.

### Parts:
- **Mouth** - Chews food, saliva starts digestion
- **Esophagus** - Tube that carries food to stomach
- **Stomach** - Mixes food with acid to break it down
- **Small Intestine** - Absorbs nutrients from food
- **Large Intestine** - Removes waste from body

### Process:
1. Chew food in mouth
2. Swallow - food goes to stomach
3. Stomach breaks down food
4. Nutrients absorbed in intestines
5. Waste removed from body

## Circulatory System
Carries blood throughout the body.

### Parts:
- **Heart** - Pumps blood around body
- **Blood vessels** - Tubes that carry blood
- **Blood** - Carries oxygen and nutrients

### Fun Facts:
- Heart beats about 100,000 times per day
- Blood travels through 60,000 miles of blood vessels
- Red blood cells carry oxygen
- White blood cells fight germs"
                },
                new LearningTopic
                {
                    Title = "Kenya's Geography",
                    Subject = "Social Studies",
                    Grade = "4-5",
                    Icon = "🗺️", 
                    Color = "#27AE60",
                    Content = @"# Kenya's Geography & Culture

## Physical Features

### Mountains:
- **Mount Kenya** - Second highest mountain in Africa
- **Mount Elgon** - On border with Uganda
- **Aberdare Ranges** - Central Kenya highlands

### Lakes:
- **Lake Victoria** - Largest lake in Africa (shared with Uganda, Tanzania)
- **Lake Turkana** - Largest desert lake in the world
- **Lake Nakuru** - Famous for flamingos
- **Lake Naivasha** - Freshwater lake with hippos

### Rivers:
- **River Tana** - Longest river in Kenya
- **River Athi** - Flows through Nairobi
- **River Ewaso Ng'iro** - Flows through northern Kenya

## Climate Zones:
- **Coastal** - Hot and humid, good for tourism
- **Highland** - Cool temperatures, good for farming
- **Arid/Semi-arid** - Hot and dry, pastoralism

## Major Cities:
- **Nairobi** - Capital city, business center
- **Mombasa** - Coastal city, main port
- **Kisumu** - Lakeside city on Lake Victoria
- **Nakuru** - Agricultural center
- **Eldoret** - Highland town, athletics center

## Tribes and Culture:
- **Kikuyu** - Largest tribe, Central Kenya
- **Luhya** - Western Kenya
- **Luo** - Around Lake Victoria
- **Kalenjin** - Rift Valley, famous runners
- **Kamba** - Eastern Kenya
- **Maasai** - Southern Kenya, pastoralists

## Wildlife:
- **Big Five**: Lion, Elephant, Buffalo, Rhino, Leopard
- **Maasai Mara** - Great wildebeest migration
- **Tsavo** - Largest national park
- **Amboseli** - Famous for elephants with Mount Kilimanjaro view"
                }
            };
        }

        private List<LearningTopic> GetUpperGradeTopics()
        {
            return new List<LearningTopic>
            {
                new LearningTopic
                {
                    Title = "Advanced Human Body Systems",
                    Subject = "Science",
                    Grade = "6-7",
                    Icon = "🧬",
                    Color = "#9B59B6",
                    Content = @"# Advanced Human Body Systems

## Nervous System
Controls all body functions and helps us think.

### Central Nervous System:
- **Brain** - Control center, processes information
  - *Cerebrum* - Thinking, memory, movement
  - *Cerebellum* - Balance and coordination  
  - *Brain stem* - Controls breathing, heart rate
- **Spinal Cord** - Carries messages between brain and body

### Peripheral Nervous System:
- **Nerves** - Carry electrical signals
- **Sensory nerves** - Bring information to brain
- **Motor nerves** - Carry commands to muscles

### How it works:
1. Senses detect information (sight, sound, touch)
2. Nerves send signals to brain
3. Brain processes information
4. Brain sends commands back through nerves
5. Body responds (move, speak, etc.)

## Excretory System
Removes waste products from the body.

### Parts:
- **Kidneys** - Filter blood, remove waste
- **Bladder** - Stores urine
- **Ureters** - Tubes from kidneys to bladder
- **Urethra** - Tube from bladder to outside
- **Skin** - Removes waste through sweat
- **Lungs** - Remove carbon dioxide

### Functions:
- Filter blood to remove toxins
- Maintain water balance in body
- Remove excess salts and minerals
- Regulate blood pressure

## Reproductive System
Allows humans to create new life.

### Male System:
- Produces sperm cells
- Hormones control development

### Female System:
- Produces egg cells
- Provides place for baby to develop
- Hormones control monthly cycle

### Puberty Changes:
- Body grows taller and stronger
- Voice changes (especially boys)
- Body develops adult characteristics
- Emotions and feelings change
- Increased responsibility and maturity

## Endocrine System
Uses hormones to control body functions.

### Major Glands:
- **Pituitary** - Master gland, controls other glands
- **Thyroid** - Controls metabolism
- **Adrenals** - Stress response, energy
- **Pancreas** - Controls blood sugar
- **Reproductive glands** - Control puberty and reproduction

### Hormones:
- Chemical messengers in blood
- Control growth, development, metabolism
- Work slower than nervous system
- Effects last longer than nerve signals"
                },
                new LearningTopic
                {
                    Title = "African History & Civilizations",
                    Subject = "Social Studies",
                    Grade = "6-7",
                    Icon = "🏛️",
                    Color = "#8E44AD",
                    Content = @"# African History & Civilizations

## Ancient African Kingdoms

### Kingdom of Kush (Sudan)
- Existed from 1070 BC to 350 AD
- Famous for iron working and gold mining
- Built pyramids like Egypt
- Conquered Egypt for a period
- Capital cities: Kerma, Napata, Meroë

### Kingdom of Aksum (Ethiopia)
- Major trading empire (100-960 AD)
- Connected Africa, Arabia, and Mediterranean
- First African kingdom to mint its own coins
- Adopted Christianity early
- Built tall stone obelisks

### Great Zimbabwe (Zimbabwe)
- Flourished 1100-1450 AD
- Built impressive stone structures
- Center of gold and ivory trade
- Name means 'stone houses'
- Influenced modern Zimbabwe's name

### Mali Empire (West Africa)
- Existed 1235-1600 AD
- Controlled trans-Saharan trade routes
- Mansa Musa - richest person in history
- Timbuktu - center of learning and trade
- Spread Islam across West Africa

### Songhai Empire (West Africa)
- Largest empire in African history
- Controlled Niger River trade
- Advanced military with cavalry
- Promoted education and scholarship
- Fell to Moroccan invasion in 1591

## Pre-Colonial Kenya

### Coastal City-States:
- **Kilwa** - Major trading port
- **Malindi** - Welcomed Vasco da Gama
- **Mombasa** - Strategic location
- **Lamu** - Preserved Swahili culture

### Interior Societies:
- **Agikuyu** - Agricultural society, Central Kenya
- **Maasai** - Pastoralist warriors, Rift Valley
- **Luo** - Fishing and farming, Lake Victoria
- **Kamba** - Long-distance traders

## Colonial Period (1895-1963)

### British Rule:
- Built Uganda Railway (1896-1901)
- Established settler economy
- Introduced cash crops (coffee, tea)
- Created reserves for African communities

### Resistance:
- **Koitalel arap Samoei** - Nandi resistance leader
- **Mekatilili wa Menza** - Giriama prophetess
- **Mau Mau** - Freedom fighters (1952-1960)

### Path to Independence:
- Formation of political parties
- Constitutional conferences in London
- Jomo Kenyatta released from prison
- Independence achieved December 12, 1963

## African Heroes & Leaders

### Ancient Leaders:
- **Queen Nzinga** - Resisted Portuguese in Angola
- **Shaka Zulu** - Military genius, South Africa
- **Sundiata Keita** - Founder of Mali Empire

### Modern Leaders:
- **Jomo Kenyatta** - Kenya's first President
- **Nelson Mandela** - Anti-apartheid leader
- **Kwame Nkrumah** - Ghana's independence leader
- **Wangari Maathai** - Environmental activist, Nobel Prize winner

## African Contributions to World:
- Mathematics and astronomy (Egypt)
- Iron working technology
- Agricultural innovations
- Art and sculpture
- Music and dance traditions
- Philosophy and oral literature"
                }
            };
        }
    }
}
