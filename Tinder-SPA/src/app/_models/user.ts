import { Photo } from './photo';

export interface User {
    /* Podstawowe informacje */
    id: number;
    username: string;
    gender: string;
    age: number;
    zodiacSign: string;
    createdAt: Date;
    lastActive: Date;
    city: string;
    country: string;
    /*zakładka info*/
    growth: string;
    eyeColor: string;
    hairColor: string;
    martialStatus: string;
    education: string;
    profession: string;
    children: string;
    languages: string;
    /*zakładka o mnie*/
    motto: string;
    description: string;
    personality: string;
    lookingFor: string;
    /*Zakładka pasje, zainteresowania*/
    interests: string;
    freeTime: string;
    sport?: any;
    movies: string;
    music: string;
    iLike: string;
    iDoNotLike: string;
    makesMeLaugh: string;
    itFeelsBestIn: string;
    friendsWouldDescribeMe: string;
    /*zakładka zdjęcia*/
    photos: Photo[];
    photoUrl: string;
}
