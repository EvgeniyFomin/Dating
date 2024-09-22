import { Photo } from "./photo"

export interface Member {
    id: number
    userName: string
    gender: number
    age: number
    mainPhotoUrl: string
    knownAs: string
    created: Date
    lastActive: Date
    introduction: string
    interests: string
    lookingFor: string
    city: string
    country: string
    photos: Photo[]
}