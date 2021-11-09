export default interface ChangePasswordDto {
    repeatNewPassword: string;
    newPassword: string;
    token: string | string[];
}
