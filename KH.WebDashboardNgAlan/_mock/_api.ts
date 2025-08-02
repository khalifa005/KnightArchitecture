import { MockRequest, MockStatusError } from '@delon/mock';

// region: mock data

const titles = ['Alipay', 'Angular', 'Ant Design', 'Ant Design Pro', 'Bootstrap', 'React', 'Vue', 'Webpack'];

const avatars = [
  'https://gw.alipayobjects.com/zos/rmsportal/WdGqmHpayyMjiEhcKoVE.png', // Alipay
  'https://gw.alipayobjects.com/zos/rmsportal/zOsKZmFRdUtvpqCImOVY.png', // Angular
  'https://gw.alipayobjects.com/zos/rmsportal/dURIMkkrRFpPgTuzkwnB.png', // Ant Design
  'https://gw.alipayobjects.com/zos/rmsportal/sfjbOqnsXXJgNCjCzDBL.png', // Ant Design Pro
  'https://gw.alipayobjects.com/zos/rmsportal/siCrBXXhmvTQGWPNLBow.png', // Bootstrap
  'https://gw.alipayobjects.com/zos/rmsportal/kZzEzemZyKLKFsojXItE.png', // React
  'https://gw.alipayobjects.com/zos/rmsportal/ComBAopevLwENQdKWiIn.png', // Vue
  'https://gw.alipayobjects.com/zos/rmsportal/nxkuOJlFJuAUhzlMTCEe.png' // Webpack
];
const covers = [
  'https://gw.alipayobjects.com/zos/rmsportal/HrxcVbrKnCJOZvtzSqjN.png',
  'https://gw.alipayobjects.com/zos/rmsportal/alaPpKWajEbIYEUvvVNf.png',
  'https://gw.alipayobjects.com/zos/rmsportal/RLwlKSYGSXGHuWSojyvp.png',
  'https://gw.alipayobjects.com/zos/rmsportal/gLaIAoVWTtLbBWZNYEMg.png'
];
const desc = [
  'That is something internal, they cannot reach, nor can they touch',
  'Hope is a good thing, maybe the best, good things never die',
  'Life is like a box of chocolates, the results are often unexpected',
  'There are so many taverns in the town, but she just walked into my tavern',
  'At that time I only thought about what I wanted, never thought about what I had'
];

const user = ['Kase', 'cipchk', 'User1', 'User2', 'User3', 'User4', 'User5', 'User6', 'User7', 'User8', 'User9', 'User10'];

// endregion

function getFakeList(count = 20): any[] {
  const list: any[] = [];
  for (let i = 0; i < count; i += 1) {
    list.push({
      id: `fake-list-${i}`,
      owner: user[i % 10],
      title: titles[i % 8],
      avatar: avatars[i % 8],
      cover: parseInt((i / 4).toString(), 10) % 2 === 0 ? covers[i % 4] : covers[3 - (i % 4)],
      status: ['active', 'exception', 'normal'][i % 3],
      percent: Math.ceil(Math.random() * 50) + 50,
      logo: avatars[i % 8],
      href: 'https://ant.design',
      updatedAt: new Date(new Date().getTime() - 1000 * 60 * 60 * 2 * i),
      createdAt: new Date(new Date().getTime() - 1000 * 60 * 60 * 2 * i),
      subDescription: desc[i % 5],
      description:
        'In the development process of middle-end products, different design specifications and implementation methods will appear, but there are often many similar pages and components among them, and these similar components will be extracted into a set of standard specifications.',
      activeUser: Math.ceil(Math.random() * 100000) + 100000,
      newUser: Math.ceil(Math.random() * 1000) + 1000,
      star: Math.ceil(Math.random() * 100) + 100,
      like: Math.ceil(Math.random() * 100) + 100,
      message: Math.ceil(Math.random() * 10) + 10,
      content:
        'Paragraph example: Ant Financial Design Platform ant.design, with minimal effort, seamlessly integrates into the Ant Financial ecosystem, providing experience solutions that span design and development. Ant Financial Design Platform ant.design, with minimal effort, seamlessly integrates into the Ant Financial ecosystem, providing experience solutions that span design and development.',
      members: [
        {
          avatar: 'https://gw.alipayobjects.com/zos/rmsportal/ZiESqWwCXBRQoaPONSJe.png',
          name: 'User1'
        },
        {
          avatar: 'https://gw.alipayobjects.com/zos/rmsportal/tBOxZPlITHqwlGjsJWaF.png',
          name: 'User2'
        },
        {
          avatar: 'https://gw.alipayobjects.com/zos/rmsportal/sBxjgqiuHMGRkIjqlQCd.png',
          name: 'User3'
        }
      ]
    });
  }

  return list;
}

function getNotice(): any[] {
  return [
    {
      id: 'xxx1',
      title: titles[0],
      logo: avatars[0],
      description: 'That is something internal, they cannot reach, nor can they touch',
      updatedAt: new Date(),
      member: 'Science Team',
      href: '',
      memberLink: ''
    },
    {
      id: 'xxx2',
      title: titles[1],
      logo: avatars[1],
      description: 'Hope is a good thing, maybe the best, good things never die',
      updatedAt: new Date('2017-07-24'),
      member: 'All Team Members',
      href: '',
      memberLink: ''
    },
    {
      id: 'xxx3',
      title: titles[2],
      logo: avatars[2],
      description: 'There are so many taverns in the town, but she just walked into my tavern',
      updatedAt: new Date(),
      member: 'Team Group',
      href: '',
      memberLink: ''
    },
    {
      id: 'xxx4',
      title: titles[3],
      logo: avatars[3],
      description: 'At that time I only thought about what I wanted, never thought about what I had',
      updatedAt: new Date('2017-07-23'),
      member: 'Programmer Daily',
      href: '',
      memberLink: ''
    },
    {
      id: 'xxx5',
      title: titles[4],
      logo: avatars[4],
      description: 'Winter is coming',
      updatedAt: new Date('2017-07-23'),
      member: 'High-end Design Team',
      href: '',
      memberLink: ''
    },
    {
      id: 'xxx6',
      title: titles[5],
      logo: avatars[5],
      description: 'Life is like a box of chocolates, the results are often unexpected',
      updatedAt: new Date('2017-07-23'),
      member: 'Computer Science Team',
      href: '',
      memberLink: ''
    }
  ];
}

function getActivities(): any[] {
  return [
    {
      id: 'trend-1',
      updatedAt: new Date(),
      user: {
        name: 'Lin Dongdong',
        avatar: avatars[0]
      },
      group: {
        name: 'High-end Design Team',
        link: 'http://github.com/'
      },
      project: {
        name: 'June Iteration',
        link: 'http://github.com/'
      },
      template: 'Created new project @{project} in @{group}'
    },
    {
      id: 'trend-2',
      updatedAt: new Date(),
      user: {
        name: 'Fu Xiaoxiao',
        avatar: avatars[1]
      },
      group: {
        name: 'High-end Design Team',
        link: 'http://github.com/'
      },
      project: {
        name: 'June Iteration',
        link: 'http://github.com/'
      },
      template: 'Created new project @{project} in @{group}'
    },
    {
      id: 'trend-3',
      updatedAt: new Date(),
      user: {
        name: 'Qu Lili',
        avatar: avatars[2]
      },
      group: {
        name: 'Middle School Girls Team',
        link: 'http://github.com/'
      },
      project: {
        name: 'June Iteration',
        link: 'http://github.com/'
      },
      template: 'Created new project @{project} in @{group}'
    },
    {
      id: 'trend-4',
      updatedAt: new Date(),
      user: {
        name: 'Zhou Xingxing',
        avatar: avatars[3]
      },
      project: {
        name: 'May Daily Iteration',
        link: 'http://github.com/'
      },
      template: 'Updated @{project} to published status'
    },
    {
      id: 'trend-5',
      updatedAt: new Date(),
      user: {
        name: 'Zhu Pianyou',
        avatar: avatars[4]
      },
      project: {
        name: 'Engineering Efficiency',
        link: 'http://github.com/'
      },
      comment: {
        name: 'Comment',
        link: 'http://github.com/'
      },
      template: 'Published @{comment} in @{project}'
    },
    {
      id: 'trend-6',
      updatedAt: new Date(),
      user: {
        name: 'Le Ge',
        avatar: avatars[5]
      },
      group: {
        name: 'Programmer Daily',
        link: 'http://github.com/'
      },
      project: {
        name: 'Brand Iteration',
        link: 'http://github.com/'
      },
      template: 'Created new project @{project} in @{group}'
    }
  ];
}

import { ROLES } from './_roles';

export const APIS = {
  '/api/list': (req: MockRequest) => getFakeList(req.queryString.count),
  '/api/notice': () => getNotice(),
  '/api/activities': () => getActivities(),
  'POST /api/auth/refresh': { msg: 'ok', token: 'new-token-by-refresh' },
  '/api/401': () => {
    throw new MockStatusError(401);
  },
  '/api/403': () => {
    throw new MockStatusError(403);
  },
  '/api/404': () => {
    throw new MockStatusError(404);
  },
  '/api/500': () => {
    throw new MockStatusError(500);
  },
  ...ROLES
};
