import { useState, useEffect } from 'react';

export default function App() {
  const [wowTokenValue, setWowTokenValue] = useState(null);
  const [affixDataValue, setaffixDataValue] = useState(null);

  useEffect(() => {
    const tokenfetchData = async () => {
      try {
        const response = await fetch('https://whatchoresapi.azurewebsites.net/api/v1/general/wowtoken');
        const data = await response.json();
        setWowTokenValue(data);
      } catch (error) {
        console.error('Error fetching WoW token value:', error);
      }
    };
    const affixFetchData = async () => {
      try {
        const response = await fetch('https://raider.io/api/v1/mythic-plus/affixes?region=us');
        const data = await response.json();
        setaffixDataValue(data);
      } catch (error) {
        console.error('Error fetching current affixes:', error);
      }
    }

    tokenfetchData();
    affixFetchData();
    return () => {};
  }, []);

  return (
    <div className="text-center">
      <h1 className="text-5xl underline">Welcome</h1>
      <div className="flex justify-center items-center mt-20">
        <h1 className="md:text-2xl sm:text-xl">
          {wowTokenValue !== null ? (
            `The WoW token is currently worth ${wowTokenValue.price} gold`
          ) : (
            <div className="animate-pulse bg-gray-300 h-8 w-96 rounded"></div>
          )}
        </h1>        
      </div> 
      <div className="mt-10 flex items-center justify-center">
        {affixDataValue !== null &&
          affixDataValue.affix_details.map((affix) => (
            <div key={affix.id} className="relative group">
              <a href={affix.wowhead_url} target='_blank'>
              <img
                className="ml-3 hover:border-slate-500 hover:border-2 hover:cursor-pointer"
                src={`https://wow.zamimg.com/images/wow/icons/large/${affix.icon}.jpg`}
                alt={affix.name}
              />
              </a>
              <div className="tooltip-text absolute bottom-full mb-2 w-auto hidden group-hover:block bg-slate-800 text-white text-xs rounded px-2 py-1 whitespace-nowrap">
                {affix.name}
              </div>
            </div>
          ))}
      </div>
    </div>
  );
  
  
}
