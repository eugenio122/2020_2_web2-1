import './Lancamentos.css';

import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Button, Table, Card, Tooltip } from 'reactstrap'
import { Segment, Icon } from 'semantic-ui-react';
import LancamentoForm from './LancamentoForm';
import { format } from 'date-fns';
import { moneyLabel } from '../../helpers/FnUtils'
import br from 'date-fns/locale/pt-BR';

export default function LancamentosList() {
    const [lancamentos, setLancamentos] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showFormLancamento, setShowFormLancamento] = useState(false);

    const [user, setUser] = useState();

    const [tooltipEdit, setTooltipEdit] = useState('');
    const [tooltipDelete, setTooltipDelete] = useState('');

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

    async function deleteLancamento(lancamentoId) {
        if (!window.confirm('Tem certeza que deseja excluir esse lançamento?')) return null

        const token = await authService.getAccessToken();
        const response = await fetch(`api/lancamentos/${lancamentoId}`, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'DELETE'
        });

        getLancamentos()
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
                    <Table responsive borderless hover>
                        <thead>
                            <tr>
                                <th>Descrição</th>
                                <th>Valor</th>
                                <th>Data</th>
                                <th>Conta</th>
                                <th />
                            </tr>
                        </thead>
                        <tbody>
                            {lancamentos.map(lancamento => (
                                <tr key={lancamento.id}>
                                    <td>{lancamento.descricao}</td>
                                    <td className={`${lancamento.despesaReceita ? 'debit' : 'credit'}`}>
                                        {`${moneyLabel(lancamento.valor)}`} <Icon name={lancamento.despesaReceita ? 'arrow down' : 'arrow up'} />
                                    </td>
                                    <td>{format(new Date(lancamento.data), 'dd/MM/yyyy', { locale: br })}</td>
                                    <td>{lancamento.conta}</td>
                                    <td>
                                        {/*<Button outline color='info' id='tooltip-edit'><Icon name='edit' /></Button>{' '}
                                        <Tooltip
                                            placement='top'
                                            isOpen={tooltipEdit === lancamento.id}
                                            target='tooltip-edit'
                                            toggle={() => !tooltipEdit ? setTooltipEdit(lancamento.id) : setTooltipEdit('')}
                                        >
                                            Editar lançamento
                                        </Tooltip>*/}
                                        <Button outline color='danger' id='tooltip-delete' onClick={() => deleteLancamento(lancamento.id)}><Icon name='trash' /></Button>
                                        <Tooltip
                                            placement='top'
                                            isOpen={tooltipDelete == lancamento.id}
                                            target='tooltip-delete'
                                            toggle={() => !tooltipDelete ? setTooltipDelete(lancamento.id) : setTooltipDelete('')}
                                        >
                                            Excluir lançamento
                                        </Tooltip>
                                    </td>
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