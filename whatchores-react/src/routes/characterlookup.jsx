import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export default function CharacterLookup() {
    const [realmData, setRealmData] = useState(null);
    const [characterName, setCharacterName] = useState('');
    const [realmName, setRealmName] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchRealmData = async () => {
            try {
                const response = await fetch('https://whatchoresapi.azurewebsites.net/localhost:7031/api/v1/general/realms');
                const data = await response.json();
                setRealmData(data);
            } catch (error) {
                console.error('Error fetching realm list:', error);
            }
        };
        fetchRealmData();
    }, []);
    
    const handleSubmit = (e) => {
        e.preventDefault();        
        if (realmName && characterName) {
            navigate(`/Character/${realmName}/${characterName}`);
        } else {            
            alert('Please enter both a realm and a character name.');
        }
    };

    return (
        <form className="flex flex-col items-center justify-center h-full pt-24" onSubmit={handleSubmit}>
            <div className="max-w-sm">
                <label htmlFor="realm" className="block text-sm font-medium leading-6 text-white">
                    Realm
                </label>
                <div className="relative mt-2 rounded-md shadow-sm">
                    <input
                        type="text"
                        name="realm"
                        id="realm"
                        list="realms"
                        className="block w-full rounded-md border-0 py-1.5 pr-20 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                        placeholder="Realm"
                        onChange={(e) => setRealmName(e.target.value)}
                    />
                    {realmData && (
                        <datalist id="realms">
                            {realmData.map(realm => (
                                <option key={realm.realmName} value={realm.realmName} />
                            ))}
                        </datalist>
                    )}
                </div>
            </div>
            <div className="max-w-sm mt-4">
                <label htmlFor="name" className="block text-sm font-medium leading-6 text-white">
                    Character Name
                </label>
                <div className="relative mt-2 rounded-md shadow-sm">
                    <input
                        type="text"
                        name="name"
                        id="name"              
                        className="block w-full rounded-md border-0 py-1.5 pr-20 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                        placeholder="Name"
                        onChange={(e) => setCharacterName(e.target.value)}
                    />          
                </div>
            </div>
            <div className="max-w-sm mt-4">                
                <button type='submit' className='bg-purple-700 hover:bg-purple-800 transition-colors rounded-md border-0 py-1.5 text-white w-64'>Submit</button>
            </div>
        </form>
    );
}
