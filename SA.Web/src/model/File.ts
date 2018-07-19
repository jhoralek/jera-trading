import { User, Record } from '@/model';
interface File {
    user: User;
    record: Record;
    path: string;
    name: string;
    created?: Date;
    id?: number;
}

interface FileShort {
    id?: number;
    name: string;
    path: string;
}

export {
    File,
    FileShort,
};
