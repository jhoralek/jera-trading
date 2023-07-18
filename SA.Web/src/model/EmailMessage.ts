import { EmailAddress } from '@/model';

export default interface EmailMessage {
    toAddresses: EmailAddress[];
    fromAddresses: EmailAddress[];
    subject: string;
    content: string;
}
