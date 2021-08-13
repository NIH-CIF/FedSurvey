import Cookies from 'universal-cookie';

// when developing, maybe worth setting this to false
const LOGIN_ENABLED = process.env.NODE_ENV === 'production';

const hasToken = () => (
    (new Cookies()).get('token')
);

export {
    LOGIN_ENABLED,
    hasToken
};