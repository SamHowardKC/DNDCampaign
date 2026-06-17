

import { useState, useEffect } from "react";
import { styles } from "../../styles/dashboard/DashboardStyle";
import type { ResultInterface } from "../../interfaces/Result";
import type { ActiveCampaign } from "../../interfaces/Campaign/CampaignInterfaces";
//import type { ResultInterface } from "../../interfaces/Result";
//import { Link, useNavigate } from "react-router-dom";


export default function Dashboard() {
    return (
        <div style={styles.container}>
            <h2 style={styles.title}>Dashboard</h2>

            <div style={styles.tableWrapper}>
                <CampaignTable />
            </div>
        </div>
    );
}

/*
To Do:
Add players level
Add average level
Add number of players
*/
function CampaignTable() {
    const [campaigns, setCampaigns] = useState<ActiveCampaign[]>([]);


    useEffect(() => {
        const loadCampaigns = async () => {
            try {
                const res = await fetch("https://dndcampaign.onrender.com/api/campaign/activeuser", {
                    credentials: "include"
                });

                const result: ResultInterface<{ campaigns: ActiveCampaign[] }> = await res.json();

                // Backend returned a failure Result<T>
                if (!result.success) {
                    console.error("Backend error:", result.error);
                    return;
                }

                // Success → extract campaigns from result.data
                setCampaigns(result.data.campaigns);
            } catch (err) {
                console.error("Failed to load campaigns", err);
            }
        };

    loadCampaigns();
    }, []);


    return (
        <table style={styles.table}>
            <thead>
                <tr>
                    <th style={styles.th}>Name</th>
                    <th style={styles.th}>Character</th>
                    <th style={styles.th}>Dungeon Master</th>
                    <th style={styles.th}>Players</th>
                </tr>
            </thead>

            <tbody>
                {campaigns.map(c => (
                    <tr
                        key={c.id}
                        style={styles.row}
                        onMouseEnter={e => e.currentTarget.style.backgroundColor = "#fafafa"}
                        onMouseLeave={e => e.currentTarget.style.backgroundColor = ""}
                    >
                        <td style={styles.td}>{c.name}</td>
                        <td style={styles.td}>{c.name ?? "-"}</td>
                        <td style={styles.td}>{c.dungeonMasterName}</td>
                        
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

/*

type Character = {
    id: string;
    name: string;
    level: string;
}

function CharacterTable() {
    const[characters, setCharacters] = useState<Character[]>([]);

    useEffect(() => {
        fetch("https://your-api-url/api/characters")
            .then(res => res.json())
            .then(data => setCharacters(data.characters))
            .catch(err => console.error("Failed to load characters", err));
    }, []);

    return (
        <table>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Class</th>
                    <th>Created at</th>
                </tr>
            </thead>

            <tbody>
                {characters.map(c => (
                    <tr key={c.id}>
                        <td>{c.name}</td>
                        <td>{c.level}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    )
} */


