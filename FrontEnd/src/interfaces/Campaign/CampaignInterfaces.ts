
export interface ActiveCampaign {
    name: string
    id: string
    dungeonMasterName: string
    isDungeonMaster: boolean
}

export interface ActiveCampaignListResponse {
    campaigns: ActiveCampaign[]
}