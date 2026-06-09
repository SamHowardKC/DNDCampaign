/*

WORK IN PROGRESS

import React, { useState } from "react";
import type { ResultInterface } from "../../interfaces/Result";
import { Link, useNavigate } from "react-router-dom";

type Character = {
    id: string;
    name: string;
    level: string;
}

type Campaign = {
    id: string;
    name: string;
    characterName: string; // nullable
    dungeonMaster: string; // name not id
    playerCount: number;
}

export default function Dashboard() {

}

function CharacterTable() {
    const[characters, setCharacters] = useState<Character[]>([]);

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
}

// will sort to campaigns where the player is the DM first
function CampaignTable() {
    const[campaigns, setCampaigns] = useState<Campaign[]>([]);

    return (
        <table>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Dungeon Master</th>
                </tr>
            </thead>
        </table>
    )
}

*/