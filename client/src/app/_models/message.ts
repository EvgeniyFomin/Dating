import { MessageUser } from "./messageUser";

export interface Message {
    id: number
    sender: MessageUser
    recipient: MessageUser
    content: string
    sentDate: Date
    readDate?: Date
}