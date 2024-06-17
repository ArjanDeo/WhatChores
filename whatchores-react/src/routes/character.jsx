import { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";

export default function Character() {
    const { realm, name } = useParams();
    const [characterData, setCharacterData] = useState(null);
    const [characterRaidData, setCharacterRaidData] = useState('');

    useEffect(() => {
        const fetchCharacterData = async () => {
            try {
                const response = await fetch(`https://whatchoresapi.azurewebsites.net/api/v1/general/charData?realm=${realm}&name=${name}&region=us`);
                const data = await response.json();
                setCharacterData(data);
            } catch (error) {
                console.error('Error fetching character data:', error);
            }
        }
        const fetchCharacterRaidData = async () => {
            try {
                const response = await fetch(`https://whatchoresapi.azurewebsites.net/api/v1/general/charRaids?realm=${realm}&name=${name}&region=us`);
                const data = await response.json();
                setCharacterRaidData(data);
            } catch (error) {
                console.error('Error fetching character data:', error);
            }
        }
        ;

        fetchCharacterData();
        fetchCharacterRaidData();
    }, [realm, name]);


    const validRaidBosses = [
        "Eranog",
        "Terros",
        "Sennarth, the Cold Breath",
        "The Primal Council",
        "Dathea, Ascended",
        "Kurog Grimtotem",
        "Broodkeeper Diurna",
        "Raszageth the Storm Eater",        
        "Kazzara, the Hellforged",
        "The Amalgamation Chamber",
        "The Forgotten Experiments",
        "Assault of the Zaqali",
        "Rashok, the Elder",
        "The Vigilant Steward, Zskarn",
        "Magmorax",
        "Echo of Neltharion",
        "Scalecommander Sarkareth",
        "Gnarlroot",
        "Igira the Cruel",
        "Volcoross",
        "Council of Dreams",
        "Larodar, Keeper of the Flame",
        "Nymue, Weaver of the Cycle",
        "Smolderon",
        "Tindral Sageswift, Seer of the Flame",
        "Fyrakk the Blazing"
    ];
    
    if (!characterData || !characterRaidData) {
        return <div>Loading...</div>;
    }

    let count = 0;
    for (let i = 0; i < characterRaidData.length; i++) {
        if (validRaidBosses.includes(characterRaidData[i])) {
            count++;
        }
}
console.log(count);
    const {
        raiderIOCharacterData: {
            name: characterName,
            guild: { name: guildName },
            gear: { item_level_equipped: itemLevel },
            active_spec_name: activeSpec,
            char_class: characterClass,
            raid_progression: {
                aberrustheshadowedcrucible,
                amirdrassilthedreamshope,
                vaultoftheincarnates
            },
            mythic_plus_scores_by_season,
            mythic_plus_weekly_highest_level_runs
        },
        classColor,
        characterMedia: {
            assets: [
            { key: mediaType1, value: mediaUrl1 },
            { key: mediaType2, value: mediaUrl2 },
            { key: mediaType3, value: mediaUrl3 }
            ]
        }
    } = characterData;

    const renderRaidProgress = amirdrassilthedreamshope ? (
        <div>
            <p className='mt-2 text-inherit'>Raid Progress (AtDH): {amirdrassilthedreamshope.summary}</p>
            <p className='text-inherit'>Raid Progress (AtSC): {aberrustheshadowedcrucible.summary}</p>
            <p className='text-inherit'>Raid Progress (VotI): {vaultoftheincarnates.summary}</p>
        </div>
    ) : (
        <p className='mt-2 text-inherit'>No Raid Progress</p>
    );
    
    let characterArmory = `https://worldofwarcraft.blizzard.com/en-us/character/us/${realm}/${name}`;
    let guildArmory = `https://worldofwarcraft.blizzard.com/en-us/guild/us/${realm}/${guildName}`

    return (
        <>
            <div style={{ color: `${classColor}` }} className="flex justify-center items-center pt-5 flex-wrap">
                <div className="max-w-md mx-auto bg-slate-800 rounded-xl shadow-md overflow-hidden md:max-w-2xl">
                    <div className="md:flex">
                        <div className="md:shrink-0">
                            <img className="h-32 w-full object-cover md:h-full md:w-48" src={characterData.raiderIOCharacterData.thumbnail_url} alt="Character Thumbnail Picture" />
                        </div>
                        <div style={{ color: `${classColor}` }} className="p-8">
                            <div className="uppercase tracking-wide text-md font-semibold"><a className='hover:text-purple-600 transition-colors' href={characterArmory}>{characterName}</a> <a className='hover:text-purple-600 transition-colors' href={guildArmory}>&lt;{guildName}&gt;</a></div>
                            <a className="block mt-1 text-lg leading-tight font-medium text-inherit hover:underline">Item Level {Math.ceil(itemLevel)} {activeSpec} {characterClass}</a>
                            {renderRaidProgress}
                            {mythic_plus_scores_by_season.map((score, key) => <p key={key}>Mythic+ Score: {score.scores.all}</p>)}
                        </div>
                    </div>
                </div>
            </div>
            <div className="overflow-x-auto pt-24 border-0 rounded-md">
                <table className="min-w-full bg-slate-800 border-0 rounded-md">
                    <thead>
                        <tr>
                            <th className="px-6 py-3 border-b border-slate-600 text-left leading-4 text-inherit">Type</th>
                            <th className="px-6 py-3 border-b border-slate-600 text-left leading-4 text-inherit">Reward Tier 1</th>
                            <th className="px-6 py-3 border-b border-slate-600 text-left leading-4 text-inherit">Reward Tier 2</th>
                            <th className="px-6 py-3 border-b border-slate-600 text-left leading-4 text-inherit">Reward Tier 3</th>
                        </tr>
                    </thead>
                    <tbody className="bg-slate-700 divide-y divide-slate-600">
                        <tr>
                            <td className="px-6 py-4 whitespace-no-wrap text-xl">Raids</td>                           
                            <td className={`px-6 py-4 whitespace-no-wrap ${count >= 2 ? 'text-lime-400' : 'text-red-600'}`}>
                                Defeat 2 Awakened Raid Bosses
                            </td>
                            <td className={`px-6 py-4 whitespace-no-wrap ${count >= 4 ? 'text-lime-400' : 'text-red-600'}`}>
                                Defeat 4 Awakened Raid Bosses
                            </td>
                            <td className={`px-6 py-4 whitespace-no-wrap ${count >= 7 ? 'text-lime-400' : 'text-red-600'}`}>
                                Defeat 7 Awakened Raid Bosses
                            </td>
                        </tr>
                        <tr>
                            <td className="px-6 py-4 whitespace-no-wrap text-xl">Dungeons ({mythic_plus_weekly_highest_level_runs.length} completed)</td>
                            <td className={`px-6 py-4 whitespace-no-wrap ${mythic_plus_weekly_highest_level_runs && mythic_plus_weekly_highest_level_runs.length >= 1 ? 'text-lime-400' : 'text-red-600'}`}>
                                Complete 1 Heroic, Mythic, or Timewalking Dungeon
                            </td>
                            <td className={`px-6 py-4 whitespace-no-wrap ${mythic_plus_weekly_highest_level_runs && mythic_plus_weekly_highest_level_runs.length >= 4 ? 'text-lime-400' : 'text-red-600'}`}>
                                Complete 4 Heroic, Mythic, or Timewalking Dungeons
                            </td>
                            <td className={`px-6 py-4 whitespace-no-wrap ${mythic_plus_weekly_highest_level_runs && mythic_plus_weekly_highest_level_runs.length >= 8 ? 'text-lime-400' : 'text-red-600'}`}>
                                Complete 8 Heroic, Mythic, or Timewalking Dungeons
                            </td>
                        </tr>                       
                    </tbody>
                </table>
            </div>
        </>
    );
}
