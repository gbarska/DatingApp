export interface Photo {
    id: number;
    url: string;
    description: string;
    dateAdded: Date;
    isMain: boolean;
    isActivated: boolean;
    // knownAs?: string;
}
