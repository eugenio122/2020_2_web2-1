import './Lancamentos.css';

import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Button, Table, Card } from 'reactstrap'
import { Segment } from 'semantic-ui-react';
import LancamentoForm from './LancamentoForm';
import { format } from 'date-fns';
import br from 'date-fns/locale/pt-BR';

export default function LancamentosList() {
    const [lancamentos, setLancamentos] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showFormLancamento, setShowFormLancamento] = useState(false)

    const [user, setUser] = useState()

    useEffect(() => {
        getLancamentos();
    }, [])

    async function getLancamentos() {
        const token = await authService.getAccessToken();
        const user = await authService.getUser()
        const response = await fetch('api/lancamentos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();
        console.log(data);

        setLancamentos(data);
        setLoading(data);
        setUser(user)
    }

    return (
        <div>
            <div className='container-header-lancamento'>
                <Segment textAlign='right' style={{ padding: 10 }}>
                    <Button color="success" onClick={() => setShowFormLancamento(true)}> Novo lançamento</Button>
                </Segment>
            </div>

            <div className='container-lancamentos'>
                <Segment color="green">
                    <Table responsive>
                        <thead>
                            <tr>
                                <th>Descrição</th>
                                <th>Valor</th>
                                <th>Data</th>
                                <th>Tipo</th>
                                <th>Conta</th>
                            </tr>
                        </thead>
                        <tbody>
                            {lancamentos.map(lancamento => (
                                <tr key={lancamento.id}>
                                    <td>{lancamento.descricao}</td>
                                    <td>{lancamento.valor}</td>
                                    <td>{format(new Date(lancamento.data), 'dd/MM/yyyy', { locale: br })}</td>
                                    <td>{`${lancamento.despesaReceita ? 'Despesa' : 'Receita'}`}</td>
                                    <td>{lancamento.conta}</td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                </Segment>
            </div>

            <LancamentoForm showFormLancamento={showFormLancamento} setShowFormLancamento={setShowFormLancamento} getLancamentos={getLancamentos} user={user} />
        </div >
    )
}
