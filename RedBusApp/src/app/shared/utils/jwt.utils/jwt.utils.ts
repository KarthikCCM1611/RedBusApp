export interface JwtPayload {
    sub?: string;
    name?: string;
    role?: string | string[]; // some backends emit array
    exp?: number;             // epoch seconds
    [key: string]: any;
}

export function decodeJwt(token: string): JwtPayload | null {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const json = decodeURIComponent(
            atob(base64).split('').map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)).join('')
        );
        return JSON.parse(json);
    } catch {
        return null;
    }
}

export function hasRole(payload: JwtPayload | null, role: string): boolean {
    if (!payload) return false;
    const ROLE_URI = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
    const NAME_URI = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const r = payload[ROLE_URI];
    if (!r) return false;
    return Array.isArray(r) ? r.includes(role) : r === role;
}

export function getValueByKey(payload: JwtPayload | null, key: string): string {
    if (!payload || !key) return '';
    const value = payload[key];
    if (!value) return '';
    return value;
}

// export function getEmail(payload: JwtPayload | null): string {
//     if (!payload) return '';
//     const EMAIL_URI = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
//     const email = payload[EMAIL_URI];
//     if (!email) return '';
//     return email;
// }

// export function getUserId(payload: JwtPayload | null): string {
//     if (!payload) return '';
//     const email = payload['UserId'];
//     if (!email) return '';
//     return email;
// }
